﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel.Tables;
using DTO = Application.DTO;
using Enums = Domain.Infrastructure;

namespace Presentation.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class DocPdf
    {
        /// <summary>
        /// DocPdf
        /// </summary>
        public DocPdf()
        {
            this.Causas = new List<DTO.Models.Causa>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string PathSave { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string MapPath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Document document { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Filename { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubTitulo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SubTitulo2 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IList<DTO.Models.Causa> Causas { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DTO.Models.Tabla Tabla { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DTO.Models.Usuario Usuario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DTO.Models.Causa Causa { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime Ahora { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsTabla { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEstadoDiario { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsExpedienteElectronico { get; set; }


        /// <summary>
        /// CreateDocument
        /// </summary>
        /// <param name="title"></param>
        public void CreateDocument(string title)
        {
            document = new Document();

            SetInfoDocument(title);
            DefineStyles();

            SetLogo();
            SetTitulos();
        }



        #region Private

        private void SimpleTable()
        {
            //document.LastSection.AddParagraph("Simple Tables", "Heading2");

            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(5));

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Itemus");
            cell = row.Cells[1];
            cell.AddParagraph("Descriptum");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("1");
            cell = row.Cells[1];
            cell.AddParagraph("asdfasdfsadfas");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("2");
            cell = row.Cells[1];
            cell.AddParagraph("asdf asdf sadf asdf asf asf asdf sadf sadf sadf");

            table.SetEdge(0, 0, 2, 3, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            document.LastSection.Add(table);
        }

        private void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Arial";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Arial";
            style.Font.Size = 16;
            style.Font.Bold = true;
            style.Font.Color = Colors.Black;
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            //style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;
            style.ParagraphFormat.SpaceBefore = 10;

            style = document.Styles["Heading2"];
            style.Font.Size = 13;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.Font.Color = Colors.Black;
            style.Font.Name = "Arial";
            style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
            style.ParagraphFormat.SpaceBefore = 10;
            style.ParagraphFormat.SpaceAfter = 20;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;


            //HeadingTabla
            style = document.Styles["Heading4"];
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Italic = false;
            style.ParagraphFormat.SpaceAfter = 10;
            style.ParagraphFormat.SpaceBefore = 10;

            //SubtitleTabla
            style = document.Styles["Heading5"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.Font.Italic = false;
            style.ParagraphFormat.SpaceAfter = 6;
            style.ParagraphFormat.SpaceBefore = 3;


            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
        }

        private void SetLogo()
        {
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.AddImage(GetPathLogo());
        }

        private void SetInfoDocument(string title)
        {
            document.Info.Author = "TDPI";
            document.Info.Comment = "";
            document.Info.Keywords = "";
            document.Info.Subject = "";
            document.Info.Title = title;

            if (IsEstadoDiario)
            {
                document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1);
                document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1.2);
            }
            else
            {
                document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1.5);
                document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1.2);
            }

            if (IsExpedienteElectronico)
            {
                document.DefaultPageSetup.LeftMargin = Unit.FromCentimeter(1.3);
                document.DefaultPageSetup.TopMargin = Unit.FromCentimeter(1.3);
                document.DefaultPageSetup.RightMargin = Unit.FromCentimeter(1.3);
                document.DefaultPageSetup.BottomMargin = Unit.FromCentimeter(1.3);
            }
        }

        private string GetPathLogo()
        {
            return $"{MapPath}\\Includes\\css\\images\\logos\\principal.png";
        }

        /// <summary>
        /// GetPathFile
        /// </summary>
        /// <returns></returns>
        public string GetPathFile()
        {
            return $"{PathSave}\\{Filename}";
        }

        private void SetTitulos()
        {

            if (IsExpedienteElectronico)
            {
                Section section = document.LastSection;

                Paragraph paragraph = section.AddParagraph($"Fecha generación: {Ahora.ToString("dd-MM-yyy HH:mm")}");
                paragraph.Format.Font.Size = 8; 
                paragraph.Format.Alignment = ParagraphAlignment.Right;
                //paragraph.Format.Font.Color = Colors.;
                //paragraph.Format.SpaceBefore = spaceAntes;
                //paragraph.Format.SpaceAfter = spaceDespues;
            }

            string styleHeader = "Heading1";
            string styleSubTitle = "Heading2";

            if (IsTabla)
            {
                styleHeader = "Heading4";
                styleSubTitle = "Heading5";

                AgregarParrafo(Tabla.Sala.Descripcion.Trim(), align: ParagraphAlignment.Right, spaceAntes: 0, bold: true);
            }

            if (IsEstadoDiario)
            {
                styleHeader = "Heading2";
            }

            document.LastSection.AddParagraph(Titulo, styleHeader);

            if (IsExpedienteElectronico)
            {
                document.LastSection.AddParagraph("");
                //document.LastSection.AddParagraph("");
            }

            if (!string.IsNullOrWhiteSpace(SubTitulo))
            {
                document.LastSection.AddParagraph(SubTitulo, styleSubTitle);
            }

            if (!string.IsNullOrWhiteSpace(SubTitulo2))
            {
                document.LastSection.AddParagraph(SubTitulo2, styleSubTitle);

                if (IsTabla)
                {
                    document.LastSection.AddParagraph("");
                    document.LastSection.AddParagraph("");
                }

            }

            //if (IsEstadoDiario)
            //{
            //    Section section = document.LastSection;
            //    section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            //    section.PageSetup.StartingNumber = 1;

            //    HeaderFooter header = section.Headers.Primary;
            //    header.AddParagraph("\tOdd Page Header");

            //    header = section.Headers.EvenPage;
            //    header.AddParagraph("Even Page Header");
            //}
            //else
            //{

            //}

        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        public void SetListadoIngreso()
        {
            List<PdfRow> Header = new List<PdfRow>();
            Header.Add(new PdfRow() { name = "N° ROL", width = 2.5f });
            Header.Add(new PdfRow() { name = "N° SOLICITUD", width = 2.5f });
            Header.Add(new PdfRow() { name = "INDIVIDUALIZACION DEL EXPEDIENTE", width = 9.5f });
            Header.Add(new PdfRow() { name = "INGRESO", width = 3.5f });

            int count = 0;

            Table table = new Table();
            table.Borders.Width = 0.75;

            foreach (var item in Header)
            {
                table.AddColumn(Unit.FromCentimeter(item.width));
            }

            Row row = table.AddRow();
            foreach (var item in Header)
            {
                row.Shading.Color = Colors.LightGray;
                row.Height = 30;
                row.VerticalAlignment = VerticalAlignment.Center;

                Cell cell = row.Cells[count];
                cell.AddParagraph(item.name);
                cell.Format.Font.Size = 9;
                count++;
            }


            foreach (var item in Causas)
            {
                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Top;

                Cell cell = row.Cells[0];
                cell.Format.Font.Size = 9;
                cell.AddParagraph(item.NumeroTicket);

                cell = row.Cells[1];
                cell.Format.Font.Size = 9;
                cell.AddParagraph(item.Numero);

                cell = row.Cells[2];
                cell.Format.Font.Size = 9;
                cell.AddParagraph(item.Denominacion);

                cell = row.Cells[3];
                cell.Format.Font.Size = 9;
                cell.AddParagraph("1.- Ingreso a Trámite");
            }

            table.SetEdge(0, 0, Header.Count, Causas.Count + 1, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);
            document.LastSection.Add(table);

            AgregarParrafo(Usuario.GetFullName(upper:true), spaceAntes: 50);
            AgregarParrafo( $"{Usuario.GetTextoByGenero("Secretari")} - {Usuario.GetTextoByGenero("Abogad")}");
            AgregarParrafo("Tribunal de Propiedad Industrial");
        }

        internal void SetListadoTabla()
        {
            Table table = new Table();
            table.Borders.Width = 0;

            table.AddColumn(Unit.FromCentimeter(1));
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(5.5));
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(5.5));

            Unit bottomSpace = Unit.FromCentimeter(0.2);
            Unit bottomSpaceBreack = Unit.FromCentimeter(0.9);

            int count = 1;
            foreach (var item in Tabla.DetalleTabla.OrderBy(x => x.Orden))
            {
                var ListaApelantes = item.Causa.Parte.Where(x => x.TipoParteID == (int)Enums.TipoParte.Apelante).ToList();
                var ListaApelados = item.Causa.Parte.Where(x => x.TipoParteID == (int)Enums.TipoParte.Apelado).ToList();

                Row row = table.AddRow();
                row.BottomPadding = bottomSpace;
                row.TopPadding = Unit.FromCentimeter(0.5);

                row.VerticalAlignment = VerticalAlignment.Top;

                #region Tipo Causa + Denominacion

                Cell cell = row.Cells[0];
                //cell.Format.Font.Size = 8;
                cell.AddParagraph($"{count})");

                cell = row.Cells[1];
                cell.Format.Font.Bold = true;
                cell.AddParagraph(item.Causa.TipoCausa.Descripcion.Trim());

                cell = row.Cells[2];
                cell.MergeRight = 2;
                cell.AddParagraph(item.Causa.Denominacion.Trim());

                #endregion



                #region Rol + Solicitud
                row = table.AddRow();
                row.BottomPadding = bottomSpace;

                cell = row.Cells[0];
                cell.AddParagraph("");

                cell = row.Cells[1];
                cell.Format.Font.Bold = true;
                cell.AddParagraph("Rol");
                
                cell = row.Cells[2];
                cell.AddParagraph(item.Causa.NumeroTicket.Trim());

                cell = row.Cells[3];
                cell.Format.Font.Bold = true;
                cell.AddParagraph("Solicitud");

                cell = row.Cells[4];
                cell.AddParagraph(item.Causa.Numero.Trim());

                #endregion

                #region Apelante

                row = table.AddRow();
                row.BottomPadding = bottomSpace;

                cell = row.Cells[0];
                cell.AddParagraph("");

                cell = row.Cells[1];
                cell.Format.Font.Bold = true;
                cell.AddParagraph("Apelante");

                if (ListaApelantes.Count > 0)
                {
                    Table tablaApleante = new Table();
                    tablaApleante.Borders.Width = 1;
                    tablaApleante.AddColumn(Unit.FromCentimeter(6.85));
                    tablaApleante.AddColumn(Unit.FromCentimeter(6.85));

                    foreach (var apelante in ListaApelantes)
                    {
                        Row rowApelante = tablaApleante.AddRow();
                        Cell cellApelante = rowApelante.Cells[0];
                        cellApelante.AddParagraph(apelante.Nombre);

                        cellApelante = rowApelante.Cells[1];
                        cellApelante.AddParagraph(apelante.NombreRepresentante);
                    }

                    cell = row.Cells[2];
                    cell.MergeRight = 2;
                    cell.Elements.Add(tablaApleante);
                }
                else
                {
                    cell = row.Cells[2];
                    cell.MergeRight = 2;
                    cell.AddParagraph("");
                }

                #endregion

                #region Apelado

                row = table.AddRow();
                row.BottomPadding = bottomSpaceBreack;

                cell = row.Cells[0];
                cell.AddParagraph("");

                cell = row.Cells[1];
                cell.Format.Font.Bold = true;
                cell.AddParagraph("Apelado");

                if (item.Causa.IsContencioso)
                {
                    Table tablaApelado = new Table();
                    tablaApelado.Borders.Width = 1;
                    tablaApelado.AddColumn(Unit.FromCentimeter(6.85));
                    tablaApelado.AddColumn(Unit.FromCentimeter(6.85));

                    foreach (var apelante in ListaApelados)
                    {
                        Row rowApelado = tablaApelado.AddRow();
                        Cell cellApelado = rowApelado.Cells[0];
                        cellApelado.AddParagraph(apelante.Nombre);

                        cellApelado = rowApelado.Cells[1];
                        cellApelado.AddParagraph(apelante.NombreRepresentante);
                    }

                    cell = row.Cells[2];
                    cell.MergeRight = 2;
                    cell.Elements.Add(tablaApelado);
                }
                else
                {
                    cell = row.Cells[2];
                    cell.MergeRight = 2;
                    cell.AddParagraph("");
                }


                #endregion

                count++;
            }

            document.LastSection.Add(table);

            string causas = (Tabla.DetalleTabla.Count == 1 ? "causa" : "causas");

            AgregarParrafo("Certifico que la presente tabla consta de " + Tabla.DetalleTabla.Count + " " + causas, 
                spaceAntes: 20, align: ParagraphAlignment.Left, spaceDespues: 40, leftIdent: 1, bold: true);

            #region Footer
            Table tableFooter = new Table();
            tableFooter.Borders.Width = 0;

            tableFooter.AddColumn(Unit.FromCentimeter(9.5));
            tableFooter.AddColumn(Unit.FromCentimeter(9.5));

            Row rowFooter = tableFooter.AddRow();

            Cell cellFooter = rowFooter.Cells[0];
            cellFooter.Format.Alignment = ParagraphAlignment.Center;
            cellFooter.Format.Font.Bold = true;
            cellFooter.AddParagraph(Domain.Infrastructure.WebConfigValues.NombrePresidente);

            cellFooter = rowFooter.Cells[1];
            cellFooter.Format.Alignment = ParagraphAlignment.Center;
            cellFooter.Format.Font.Bold = true;
            cellFooter.AddParagraph(Usuario.GetFullName(upper: true));

            rowFooter = tableFooter.AddRow();

            cellFooter = rowFooter.Cells[0];
            cellFooter.Format.Alignment = ParagraphAlignment.Center;
            cellFooter.Format.Font.Bold = true;
            cellFooter.AddParagraph("Presidente");

            cellFooter = rowFooter.Cells[1];
            cellFooter.Format.Alignment = ParagraphAlignment.Center;
            cellFooter.Format.Font.Bold = true;
            cellFooter.AddParagraph($"{Usuario.GetTextoByGenero("Secretari")} - {Usuario.GetTextoByGenero("Abogad")}");

            document.LastSection.Add(tableFooter);

            #endregion

        }

        private void AgregarParrafo(string text,  ParagraphAlignment align = ParagraphAlignment.Center, 
            int spaceAntes = 0, bool bold = false, int spaceDespues = 0, int leftIdent = 0)
        {
            Section section = document.LastSection;

            Paragraph paragraph = section.AddParagraph(text);
            //paragraph.Format.Font.Size = size;
            paragraph.Format.Font.Bold = bold;
            paragraph.Format.Alignment = align;
            paragraph.Format.SpaceBefore = spaceAntes;
            paragraph.Format.SpaceAfter = spaceDespues;
            if (align == ParagraphAlignment.Left)
            {
                paragraph.Format.LeftIndent = Unit.FromCentimeter(leftIdent);
            }

            
        }


        /// <summary>
        /// Render document
        /// </summary>
        public void Render()
        {
            document.UseCmykColor = true;

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;
            pdfRenderer.RenderDocument();

            pdfRenderer.PdfDocument.Save(GetPathFile());
        }

        internal void SetListadoEstadoDiario()
        {
            List<PdfRow> Header = new List<PdfRow>();
            Header.Add(new PdfRow() { name = "N°", align = ParagraphAlignment.Center, width = 0.5f });
            Header.Add(new PdfRow() { name = "N° ROL", width = 1.5f });
            Header.Add(new PdfRow() { name = "ROL DE CAUSA EN PALABRAS", width = 2.2f });
            Header.Add(new PdfRow() { name = "N° SOLICITUD", width = 1.8f });
            Header.Add(new PdfRow() { name = "APELANTES\nRECURRENTES\nSOLICITANTES", width = 2.5f });
            Header.Add(new PdfRow() { name = "APELADOS\nRECURRIDOS", width = 2.5f });
            Header.Add(new PdfRow() { name = "INDIVIDUALIZACIÓN", width = 3.5f });
            Header.Add(new PdfRow() { name = "N° RESOL.", align = ParagraphAlignment.Center, width = 1.1f });
            Header.Add(new PdfRow() { name = "TEXTO RESOLUCIÓN", width = 3.8f });

            Moneda moneda = new Moneda();
            int count = 0;

            Table table = new Table();
            table.Borders.Width = 0.75;

            foreach (var item in Header)
            {
                table.AddColumn(Unit.FromCentimeter(item.width));
            }

            Row row = table.AddRow();
            foreach (var item in Header)
            {
                row.VerticalAlignment = VerticalAlignment.Center;
                row.HeadingFormat = true;

                Cell cell = row.Cells[count];
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.Format.Font.Bold = true;
                cell.AddParagraph(item.name);
                cell.Format.Font.Size = 8;
                count++;
            }


            #region Detalle

            count = 1;
            float fontCellSize = 8;

            foreach (var item in Causas)
            {
                row = table.AddRow();
                row.VerticalAlignment = VerticalAlignment.Top;

                Cell cell = row.Cells[0];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(count.ToString());

                cell = row.Cells[1];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(item.NumeroTicket.Trim());


                string[] rol = item.NumeroTicket.Split('-');
                List<string> palabras = new List<string>();
                for (int i = 0; i < rol.Length; i++)
                {
                    int rolNum = int.Parse(rol[i]);
                    palabras.Add(moneda.Convertir(rolNum.ToString(), true, ""));
                }
                cell = row.Cells[2];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(string.Join(" ", palabras));

                cell = row.Cells[3];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(item.Numero.Trim());

                cell = row.Cells[4];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(item.GetParteCausa(Enums.TipoParte.Apelante));

                cell = row.Cells[5];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(item.GetParteCausa(Enums.TipoParte.Apelado));

                cell = row.Cells[6];
                cell.Format.Font.Size = fontCellSize;
                cell.AddParagraph(item.Denominacion.Trim());

                cell = row.Cells[7];
                cell.Format.Font.Size = fontCellSize;
                cell.Format.Alignment = ParagraphAlignment.Center;
                cell.AddParagraph(item.Expediente.Count.ToString());

                cell = row.Cells[8];
                cell.Format.Font.Size = fontCellSize;
                foreach (var ex in item.Expediente)
                {
                    cell.AddParagraph(ex.Observacion.Trim());
                    cell.AddParagraph("");
                }
                
                count++;
            }


            #endregion


            //table = document.LastSection.Headers.Primary.AddTable();
            document.LastSection.Add(table);


            

            string LastRol = Causas.LastOrDefault().NumeroTicket.Trim();
            int NumExpedientes = Causas.Count;

            //Paragraph foot = document.LastSection.Footers.Primary.AddParagraph();
            //foot.AddText("Page ");
            //foot.AddPageField();
            //foot.AddText(" of ");
            //foot.AddNumPagesField();

            DocumentRenderer docRenderer = new DocumentRenderer(document);
            docRenderer.PrepareDocument();
            int NumPaginas = docRenderer.FormattedDocument.PageCount;


            string TxtExpediente = string.Format("{0} {1}", NumExpedientes, (NumExpedientes == 1) ? "expediente correspondiente" : "expedientes correspondientes");
            string FechaPalabras = string.Format("{0} de {1} del año {2}",
                moneda.Convertir(Ahora.Day.ToString(), false, ""),
                Ahora.ToString("MMMM"),
                moneda.Convertir(Ahora.Year.ToString(), false, ""));


            string TxtNumPaginas = string.Format("{0} {1}", NumPaginas, (NumPaginas == 1) ? "página." : "páginas.");

            string TxtFooter = @"Certifico que el presente estado se cierra con la causa Rol N° {0}, contiene {1} al Estado Diario de fecha {2} y consta de {3} ";

            string parrafoFinal = string.Format(TxtFooter, LastRol, TxtExpediente, FechaPalabras, TxtNumPaginas);

            AgregarParrafo(parrafoFinal, spaceAntes: 20, align: ParagraphAlignment.Left, spaceDespues: 30);

            AgregarParrafo(Usuario.GetFullName(upper: true), spaceAntes: 50, bold: true);
            AgregarParrafo($"{Usuario.GetTextoByGenero("Secretari")}-{Usuario.GetTextoByGenero("Abogad")}", bold: true);
            AgregarParrafo("Tribunal de Propiedad Industrial", bold: true);
        }

        internal void SetExpedienteElectronico()
        {
            #region Data

            int count = 0;
            var appCommon = new Application.Services.CommonAppServices();

            int TipoDocumentoCausa = (int)Enums.TipoDocumento.Causa;

            DTO.Models.ConfTipoCausa config = appCommon.GetConfTipoCausa(Causa.TipoCausaID);

            #endregion

            //document.LastSection.AddParagraph("");

            double PageWidth = document.DefaultPageSetup.PageWidth.Centimeter - 2.2;

            Table table = new Table();
            table.Borders.Width = 0.8;

            table.AddColumn(Unit.FromCentimeter(PageWidth));

            Row row = table.AddRow();
            row.Shading.Color = Color.FromRgb(230, 230, 230);// { Argb = "ASDFASD" }; Colors.LightGray;

            Cell cell = row.Cells[0];
            cell.Format.Alignment = ParagraphAlignment.Left;
            cell.Format.Font.Bold = true;
            cell.Format.SpaceAfter = 3;
            cell.Format.SpaceBefore = 3;
            cell.Format.LeftIndent = 3;
            cell.Format.Font.Size = 11;
            cell.AddParagraph("CAUSA");

            double widthCoumn = PageWidth / 4;

            Table _t = new Table();
            _t.Borders.Width = 0;
            _t.AddColumn(Unit.FromCentimeter(widthCoumn));
            _t.AddColumn(Unit.FromCentimeter(widthCoumn));
            _t.AddColumn(Unit.FromCentimeter(widthCoumn));
            _t.AddColumn(Unit.FromCentimeter(widthCoumn));
            _t.Format.SpaceAfter = 5;
            _t.Format.SpaceBefore = 5;
            _t.RightPadding = 5;

            Row _row = _t.AddRow();
            Cell _cell = _row.Cells[0];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("Número de Rol de TDPI");
            _cell = _row.Cells[1];
            _cell.AddParagraph(": " + Causa.NumeroTicket);
            _cell = _row.Cells[2];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("Fecha de Recepción TDPI");
            _cell = _row.Cells[3];
            _cell.AddParagraph(": " + Causa.FechaIngreso.ToString("dd-MM-yyyy"));

            _row = _t.AddRow();
            _cell = _row.Cells[0];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("Tipo de Expediente");
            _cell = _row.Cells[1];
            _cell.AddParagraph(": " + Causa.TipoCausa.Descripcion.Trim());
            _cell = _row.Cells[2];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph(Causa.IsContencioso ? "Tipo Contencioso" : "");
            _cell = _row.Cells[3];
            _cell.AddParagraph(Causa.IsContencioso ? ": " + Causa.TipoContencioso.Descripcion.Trim() : "");

            _row = _t.AddRow();
            _cell = _row.Cells[0];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("N° de Solicitud");
            _cell = _row.Cells[1];
            _cell.AddParagraph(": " + Causa.Numero.Trim());
            _cell = _row.Cells[2];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("N° de Registro");
            _cell = _row.Cells[3];
            _cell.AddParagraph(": " + Causa.NumeroRegistro.Trim());

            _row = _t.AddRow();
            _cell = _row.Cells[0];
            _cell.Format.Font.Bold = true;
            _cell.AddParagraph("Individualización");
            _cell.MergeRight = 3;
            _row = _t.AddRow();
            _cell = _row.Cells[0];
            _cell.AddParagraph(Causa.Denominacion.Trim());
            _cell.MergeRight = 3;

            if (!string.IsNullOrWhiteSpace(Causa.Observacion))
            {
                _row = _t.AddRow();
                _cell = _row.Cells[0];
                _cell.Format.Font.Bold = true;
                _cell.AddParagraph("Observación");
                _cell.MergeRight = 3;
                _row = _t.AddRow();
                _cell = _row.Cells[0];
                _cell.AddParagraph(Causa.Observacion.Trim());
                _cell.MergeRight = 3;
            }

            //18.8

            if (Causa.DocumentoCausa.Count > 0)
            {
                _row = _t.AddRow();
                _cell = _row.Cells[0];
                _cell.Format.Font.Bold = true;
                _cell.AddParagraph("Documentos adjuntos");
                _cell.MergeRight = 3;

                List<PdfRow> Header = new List<PdfRow>();
                Header.Add(new PdfRow() { name = "#", width = Unit.FromCentimeter(.6) });
                Header.Add(new PdfRow() { name = "Nombre Documento", width = Unit.FromCentimeter(5) });
                Header.Add(new PdfRow() { name = "Descripción", width = Unit.FromCentimeter(6.1) });
                Header.Add(new PdfRow() { name = "Fecha Carga", width = Unit.FromCentimeter(4) });
                Header.Add(new PdfRow() { name = "Descarga", width = Unit.FromCentimeter(2.5) });

                Table _ti = new Table();
                _ti.Borders.Width = .1;
               // _ti.Format.SpaceAfter = 30;
                
                int sizeCell = 10;

                foreach (var item in Header)
                {
                    _ti.AddColumn(item.width);
                }

                Row _rowTi = _ti.AddRow();

                foreach (var item in Header)
                {
                    Cell _cellTi = _rowTi.Cells[count];
                    _cellTi.Format.Font.Bold = true;
                    _cellTi.Format.Alignment = ParagraphAlignment.Center;
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.AddParagraph(item.name);
                    count++;
                }

                count = 0;
                foreach (var item in Causa.DocumentoCausa)
                {
                    _rowTi = _ti.AddRow();
                    Cell _cellTi = _rowTi.Cells[0];
                    _cellTi.Format.Alignment = ParagraphAlignment.Center;
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.AddParagraph((count + 1).ToString());

                    _cellTi = _rowTi.Cells[1];
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.AddParagraph(item.NombreArchivoFisico.Trim());

                    _cellTi = _rowTi.Cells[2];
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.AddParagraph(item.Descripcion.Trim());

                    _cellTi = _rowTi.Cells[3];
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.Format.Alignment = ParagraphAlignment.Center;
                    _cellTi.AddParagraph(item.Fecha.Value.ToString("dd-MM-yyyy HH:mm"));

                    _cellTi = _rowTi.Cells[4];
                    _cellTi.Format.Font.Size = sizeCell;
                    _cellTi.Format.Alignment = ParagraphAlignment.Center;

                    Paragraph linkParagraph = _cellTi.AddParagraph();
                    linkParagraph.Format.Font.Color = Colors.Blue;

                    string SistemaPublicoURL = Domain.Infrastructure.WebConfigValues.SistemaPublicoURL;
                    string HashUrlPass = System.Uri.EscapeDataString(item.Hash.Trim());
                    string DownloadURL = $"{SistemaPublicoURL}ES/Uploader/GetFile?vs={item.VersionEncriptID}&hash={HashUrlPass}&td={TipoDocumentoCausa}";

                    var h = linkParagraph.AddHyperlink(DownloadURL, HyperlinkType.Web);
                    h.AddFormattedText("Descarga");

                    count++;
                }

                _row = _t.AddRow();
                _cell = _row.Cells[0];
                _cell.MergeRight = 3;
                _cell.Elements.Add(_ti);

                _row = _t.AddRow();
                _cell = _row.Cells[0];
                _cell.AddParagraph("");
                _cell.MergeRight = 3;
            }


            row = table.AddRow();
            cell = row.Cells[0];
            cell.Elements.Add(_t);



            //cell.Elements.AddParagraph("Shading", "Heading3");
            //Paragraph paragraph = new Paragraph();
            //paragraph.Format.Shading.Color = Colors.LightCoral;
            //paragraph.AddText(Causa.Denominacion.Trim());
            //cell.Elements.Add(paragraph);

            //_cell.Borders.Top = new Border() { Width = 1 };

            //row = table.AddRow();
            //cell = row.Cells[0];
            //cell.AddParagraph();
            //cell = row.Cells[1];
            //cell.AddParagraph(Causa.NumeroTicket);



            //table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            document.LastSection.Add(table);
            document.LastSection.AddParagraph("");


            //table = new Table();
            //table.Borders.Width = 0.8;

            //table.AddColumn(Unit.FromCentimeter(PageWidth));

            //row = table.AddRow();
            //row.Shading.Color = Colors.LightGray;

            //cell = row.Cells[0];
            //cell.Format.Alignment = ParagraphAlignment.Left;
            //cell.Format.Font.Bold = true;
            //cell.Format.SpaceAfter = 3;
            //cell.Format.SpaceBefore = 3;
            //cell.Format.LeftIndent = 3;
            //cell.AddParagraph("EXPEDIENTE => " + PageWidth);


            //tablaApleante = new Table();
            //tablaApleante.Borders.Width = 0;
            //tablaApleante.AddColumn(Unit.FromCentimeter(6.85));
            //tablaApleante.AddColumn(Unit.FromCentimeter(6.85));
            //tablaApleante.Format.SpaceAfter = 5;
            //tablaApleante.Format.SpaceBefore = 5;

            //for (int i = 0; i < 5; i++)
            //{
            //    Row rowApelante = tablaApleante.AddRow();
            //    Cell cellApelante = rowApelante.Cells[0];
            //    cellApelante.AddParagraph("dfasdfasdfasd => " + i);

            //    cellApelante = rowApelante.Cells[1];
            //    cellApelante.AddParagraph("NombreRepresentante=> " + i);
            //}

            //row = table.AddRow();
            //cell = row.Cells[0];
            //cell.Elements.Add(tablaApleante);

            //document.LastSection.Add(table);
            //document.LastSection.AddParagraph("");
        }



    }


    /// <summary>
    /// 
    /// </summary>
    public class PdfRow
    {
        /// <summary>
        /// 
        /// </summary>
        public PdfRow()
        {
            this.align = ParagraphAlignment.Left;
        }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ParagraphAlignment align { get; set; }
    }
}