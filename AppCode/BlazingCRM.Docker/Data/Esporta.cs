using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace BlazingCRM.Docker.Data
{
    public class Esporta
    {
        readonly SqlDbContext dbc = new SqlDbContext();

        public IEnumerable<Cliente> ElencoClienti()
        {
            try
            {
                return dbc.Clienti.ToList();
            }
            catch { throw; }
        }

        public async Task<bool> Excel(IJSRuntime iJSRuntime)
        {
            byte[] fileContents;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var workSheet = package.Workbook.Worksheets.Add("Foglio1");
                workSheet.Cells[1, 1].Value = "TIPOLOGIA";
                workSheet.Cells[1, 2].Value = "AZIENDA";
                workSheet.Cells[1, 3].Value = "COGNOME";
                workSheet.Cells[1, 4].Value = "NOME";
                workSheet.Cells[1, 5].Value = "INDIRIZZO";
                workSheet.Cells[1, 6].Value = "COMUNE";
                workSheet.Cells[1, 7].Value = "TELEFONO";
                workSheet.Cells[1, 8].Value = "EMAIL";
                int recordIndex = 2;
                foreach (var clienti in ElencoClienti())
                {
                    workSheet.Cells[recordIndex, 1].Value = clienti.Tipologia;
                    workSheet.Cells[recordIndex, 2].Value = clienti.Azienda;
                    workSheet.Cells[recordIndex, 3].Value = clienti.Cognome;
                    workSheet.Cells[recordIndex, 4].Value = clienti.Nome;
                    workSheet.Cells[recordIndex, 5].Value = clienti.Indirizzo;
                    workSheet.Cells[recordIndex, 6].Value = clienti.Comune;
                    workSheet.Cells[recordIndex, 7].Value = clienti.Telefono;
                    workSheet.Cells[recordIndex, 8].Value = clienti.Email;
                    recordIndex++;
                }
                int firstRow = 1;
                int lastRow = workSheet.Dimension.End.Row;
                int firstColumn = 1;
                int lastColumn = workSheet.Dimension.End.Column;
                ExcelRange rg = workSheet.Cells[firstRow, firstColumn, lastRow, lastColumn];
                string tableName = "Table1";
                ExcelTable tab = workSheet.Tables.Add(rg, tableName);
                tab.TableStyle = TableStyles.Medium7;
                workSheet.View.ShowGridLines = false;
                fileContents = package.GetAsByteArray();
            }
            try
            {
                var NomeFile = "Elenco_Clienti-" + DateTime.Now.ToString() + ".xlsx";
                await iJSRuntime.InvokeAsync<Esporta>("saveAsFile", NomeFile, Convert.ToBase64String(fileContents));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
