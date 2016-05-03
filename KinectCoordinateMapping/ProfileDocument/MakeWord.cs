using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace KinectCoordinateMapping.ProfileDocument
{
    public class MakeWord  //https://support.microsoft.com/en-us/kb/316384
    {
        public void WriteWord(List<Double> Data, bool AnteriorChecked, bool LeftLateralChecked, bool PosteriorChecked)
        {
            #region setting
            Application oWord;
            Object oMissing = System.Reflection.Missing.Value;
            Object oEndOfDoc = "\\endofdoc";
            oWord = new Microsoft.Office.Interop.Word.Application();
            oWord.Visible = true;    //執行過程不在畫面上開啟 Word
            _Document oDoc;

            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            string myDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);  //路徑一：我的桌面
            //string myDesktopPath = Environment.CurrentDirectory; // //路徑二：在SkeletonBasics-WPF\bin\Debug

            //【加入一段文字】
            Paragraph oPara;
            object oRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
            oPara = oDoc.Content.Paragraphs.Add(ref oRng);
            oPara.Range.Text = "Posture Analysis System Report";
            oPara.Range.Font.Bold = 1;
            oPara.Format.SpaceAfter = 6;
            oPara.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
            oPara.Range.InsertParagraphAfter();

            #endregion

            Table oTable;
            Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            //if (RadioAnterior.IsChecked == true)
            if (AnteriorChecked)
            {
                #region Anterior

                //【插入表格】    
                wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                oTable = oDoc.Tables.Add(wrdRng, 12, 5, ref oMissing, ref oMissing);  // row , col
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                //myTable.Range.Font.Name = "標楷體";
                oTable.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                oTable.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

                oTable.Cell(1, 1).Range.Text = "Patient NAME";
                oTable.Cell(1, 2).Range.Text = "Patient ID";
                oTable.Cell(1, 3).Range.Text = "Patient Age";
                oTable.Cell(1, 4).Range.Text = "Gender";
                oTable.Cell(1, 5).Range.Text = "Date";

                oTable.Cell(2, 1).Range.Text = "Nan";
                oTable.Cell(2, 2).Range.Text = "Nan";
                oTable.Cell(2, 3).Range.Text = "Nan";
                //if (RadioMale.IsChecked == true)
                //    oTable.Cell(2, 4).Range.Text = "Male";
                //if (RadioFemale.IsChecked == true)
                oTable.Cell(2, 4).Range.Text = "Nan";

                oTable.Cell(2, 5).Range.Text = DateTime.Now.ToShortDateString();

                

                oTable.Cell(8, 1).Range.Text = "Anterior View";
                oTable.Cell(3, 2).Range.Text = "Head";
                oTable.Cell(6, 2).Range.Text = "Trunk";
                oTable.Cell(10, 2).Range.Text = "Lower limbs";



                oTable.Columns[1].Cells[3].Merge(oTable.Columns[1].Cells[12]); //merge column 1  row 4
                oTable.Columns[2].Cells[4].Merge(oTable.Columns[2].Cells[6]);  //merge column 2.Trumk row 5~7
                oTable.Columns[2].Cells[5].Merge(oTable.Columns[2].Cells[10]); //merge column 3.Lower Limbs         
                //oTable.Columns[5].Cells[3].Merge(oTable.Columns[8].Cells[3]);
                //myTable.Columns[1].Cells[3].Merge(myTable.Columns[4].Cells[3]); //merge row3



                //畫表格
                oTable.Cell(3, 3).Range.Text = "兩耳水平夾角";
                oTable.Cell(4, 3).Range.Text = "兩肩水平夾角";
                oTable.Cell(5, 3).Range.Text = "左右ASIS水平夾角";
                oTable.Cell(6, 3).Range.Text = "肩膀與兩側ASIS平行角度";
                oTable.Cell(7, 3).Range.Text = "右腳外側角度";
                oTable.Cell(8, 3).Range.Text = "左腳外側角度";
                oTable.Cell(9, 3).Range.Text = "下肢長度差(右-左)(cm)";
                oTable.Cell(10, 3).Range.Text = "膝蓋水平夾角";
                oTable.Cell(11, 3).Range.Text = "右腳Q角度";
                oTable.Cell(12, 3).Range.Text = "左腳Q角度";


                //oTable.Cell(4, 5).Range.Text = "兩耳與中垂線距離";
                //oTable.Cell(5, 5).Range.Text = "兩肩與中垂線距離";
                //oTable.Cell(6, 5).Range.Text = "ASIS與中垂線距離";
                //oTable.Cell(7, 5).Range.Text = "大腿軸心與中垂線距離";
                //oTable.Cell(8, 5).Range.Text = "膝蓋與中垂線距離";
                //oTable.Cell(9, 5).Range.Text = "左右腿長度";
                //oTable.Cell(10, 5).Range.Text = "";
                //oTable.Cell(11, 5).Range.Text = "";
                //oTable.Cell(12, 5).Range.Text = "";
                //oTable.Cell(13, 5).Range.Text = "";

                //for (int i = 4; i <= 9; i++) { oTable.Cell(i, 7).Range.Text = "高度差(L-R)"; }

                try
                {
                    //灌資料
                    for (int i = 1; i <= 10; i++) { oTable.Cell(i + 3, 4).Range.Text = Data[i].ToString("f2"); }

                    //oTable.Cell(4, 6).Range.Text = "左：" + Data[31].ToString("f2") + "右：" + Data[32].ToString("f2");
                    //oTable.Cell(5, 6).Range.Text = "左：" + Data[34].ToString("f2") + "右：" + Data[35].ToString("f2");
                    //oTable.Cell(6, 6).Range.Text = "左：" + Data[37].ToString("f2") + "右：" + Data[38].ToString("f2");
                    //oTable.Cell(7, 6).Range.Text = "左：" + Data[40].ToString("f2") + "右：" + Data[41].ToString("f2");
                    //oTable.Cell(8, 6).Range.Text = "左：" + Data[43].ToString("f2") + "右：" + Data[44].ToString("f2");
                    //oTable.Cell(9, 6).Range.Text = "左：" + Data[24].ToString("f2") + "右：" + Data[25].ToString("f2");

                    //oTable.Cell(4, 8).Range.Text = Data[33].ToString("f2");
                    //oTable.Cell(5, 8).Range.Text = Data[36].ToString("f2");
                    //oTable.Cell(6, 8).Range.Text = Data[39].ToString("f2");
                    //oTable.Cell(7, 8).Range.Text = Data[42].ToString("f2");
                    //oTable.Cell(8, 8).Range.Text = Data[45].ToString("f2");
                    //oTable.Cell(9, 8).Range.Text = Data[7].ToString("f2");

                    //【插入圖片】
                    //wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    //if (AnteriorChecked)
                    //    wrdRng.InlineShapes.AddPicture(System.IO.Path.Combine(myDesktopPath, "Anterior.jpg"), ref oMissing, ref oMissing, ref oMissing);
                   
                }
                catch { }
                #endregion

                #region NextPage
                //Keep inserting text. When you get to 7 inches from top of the
                //document, insert a hard page break.
                object oPos;
                double dPos = oWord.InchesToPoints(7);
                oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
                do
                {
                    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    wrdRng.ParagraphFormat.SpaceAfter = 6;
                    //wrdRng.InsertAfter("A line of text");
                    wrdRng.InsertParagraphAfter();
                    oPos = wrdRng.get_Information
                                   (WdInformation.wdVerticalPositionRelativeToPage);
                }
                while (dPos >= Convert.ToDouble(oPos));
                object oCollapseEnd = WdCollapseDirection.wdCollapseEnd;
                object oPageBreak = WdBreakType.wdPageBreak;
                wrdRng.Collapse(ref oCollapseEnd);
                wrdRng.InsertBreak(ref oPageBreak);
                wrdRng.Collapse(ref oCollapseEnd);
                /////////////////////////////////////////////Next Page
                #endregion
            }

            if (LeftLateralChecked)
            {
                #region LeftLateral

                //【插入表格】    
                wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                oTable = oDoc.Tables.Add(wrdRng, 8, 5, ref oMissing, ref oMissing);  // row , col
                oTable.Range.ParagraphFormat.SpaceAfter = 6;
                //myTable.Range.Font.Name = "標楷體";
                oTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                oTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;

                oTable.Cell(4, 1).Range.Text = "Left lateral View";
                oTable.Cell(1, 2).Range.Text = "Head";
                oTable.Cell(4, 2).Range.Text = "Trunk";
                oTable.Cell(7, 2).Range.Text = "Lower limbs";



                oTable.Columns[1].Cells[1].Merge(oTable.Columns[1].Cells[8]); //merge  (c1,r1~8)
                oTable.Columns[2].Cells[1].Merge(oTable.Columns[2].Cells[2]); //merge  (c2,r1~2)
                oTable.Columns[2].Cells[2].Merge(oTable.Columns[2].Cells[5]); //merge  (c2,r3~6) 
                oTable.Columns[2].Cells[3].Merge(oTable.Columns[2].Cells[4]); //merge  (c2,r7~8) 
                //myTable.Columns[1].Cells[3].Merge(myTable.Columns[4].Cells[3]); //merge row3

                //寫表格標題
                oTable.Cell(1, 3).Range.Text = "頭前傾角度(2,8,Hor)";
                oTable.Cell(2, 3).Range.Text = "耳、肩膀垂直夾角(5,2,Ver)";
                oTable.Cell(3, 3).Range.Text = "耳、股骨垂直夾角(5,23,Ver)";
                oTable.Cell(4, 3).Range.Text = "肩、股骨、腳踝三點夾角(5,23,30)";
                oTable.Cell(5, 3).Range.Text = "肩、腳踝垂直夾角(5,30,Ver)";
                oTable.Cell(6, 3).Range.Text = "ASIS、PSIS骨盆傾斜角度(21,22,Hor)";
                oTable.Cell(7, 3).Range.Text = "下肢外側角度(23,24,30)";
                oTable.Cell(8, 3).Range.Text = "腳踝水平夾角(24,30,Hor)";

                //oTable.Cell(1, 5).Range.Text = "耳與中垂線距離";
                //oTable.Cell(2, 5).Range.Text = "肩峰與中垂線距離";
                //oTable.Cell(3, 5).Range.Text = "PSIS與中垂線距離";
                //oTable.Cell(4, 5).Range.Text = "ASIS與中垂線距離";
                //oTable.Cell(5, 5).Range.Text = "股骨凸隆與中垂線距離";
                //oTable.Cell(6, 5).Range.Text = "膝外側與中垂線距離";

                //oTable.Cell(3, 7).Range.Text = "高度差(PSIS-ASIS)";

                try
                {

                    //灌資料
                    for (int i = 11; i <= 18; i++) { oTable.Cell(i - 10, 4).Range.Text = Data[i].ToString("f2"); }

                    //oTable.Cell(1, 6).Range.Text = Data[46].ToString("f2");
                    //oTable.Cell(2, 6).Range.Text = Data[47].ToString("f2");
                    //oTable.Cell(3, 6).Range.Text = Data[49].ToString("f2");
                    //oTable.Cell(4, 6).Range.Text = Data[50].ToString("f2");
                    //oTable.Cell(5, 6).Range.Text = Data[52].ToString("f2");
                    //oTable.Cell(6, 6).Range.Text = Data[53].ToString("f2");

                    //oTable.Cell(3, 8).Range.Text = Data[51].ToString("f2");//PSIS-ASIS高度差


                    //【插入圖片】           
                    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    //if (LeftLateralChecked == true)
                    //    wrdRng.InlineShapes.AddPicture(System.IO.Path.Combine(myDesktopPath, "LeftLateral.jpg"), ref oMissing, ref oMissing, ref oMissing);

                }
                catch { }
                #endregion

                #region NextPage
                //Keep inserting text. When you get to 7 inches from top of the
                //document, insert a hard page break.
                object oPos;
                double dPos = oWord.InchesToPoints(7);
                oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range.InsertParagraphAfter();
                do
                {
                    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    wrdRng.ParagraphFormat.SpaceAfter = 6;
                    //wrdRng.InsertAfter("A line of text");
                    wrdRng.InsertParagraphAfter();
                    oPos = wrdRng.get_Information
                                   (WdInformation.wdVerticalPositionRelativeToPage);
                }
                while (dPos >= Convert.ToDouble(oPos));
                object oCollapseEnd = WdCollapseDirection.wdCollapseEnd;
                object oPageBreak = WdBreakType.wdPageBreak;
                wrdRng.Collapse(ref oCollapseEnd);
                wrdRng.InsertBreak(ref oPageBreak);
                wrdRng.Collapse(ref oCollapseEnd);
                /////////////////////////////////////////////Next Page
                #endregion

            }

            if (PosteriorChecked)
            {
                #region Posterior


                //【插入表格】    
                Table PosteriorTable;
                PosteriorTable = oDoc.Tables.Add(wrdRng, 5, 5, ref oMissing, ref oMissing);  // row , col
                //myTable.Range.Font.Name = "標楷體";
                PosteriorTable.Borders.InsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;
                PosteriorTable.Borders.OutsideLineStyle = Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle;

                PosteriorTable.Cell(4, 1).Range.Text = "Posterior View";
                PosteriorTable.Cell(2, 2).Range.Text = "Trunk";
                PosteriorTable.Cell(4, 2).Range.Text = "Lower limbs";



                PosteriorTable.Columns[1].Cells[1].Merge(PosteriorTable.Columns[1].Cells[5]); //merge  (c1,r1~5)
                PosteriorTable.Columns[2].Cells[1].Merge(PosteriorTable.Columns[2].Cells[3]); //merge  (c2,r1~3)
                PosteriorTable.Columns[2].Cells[2].Merge(PosteriorTable.Columns[2].Cells[3]); //merge  (c2,r4~5) 

                //寫表格標題
                PosteriorTable.Cell(1, 3).Range.Text = "上背夾角";
                PosteriorTable.Cell(2, 3).Range.Text = "左肩胛骨夾角";
                PosteriorTable.Cell(3, 3).Range.Text = "右肩胛骨夾角";
                PosteriorTable.Cell(4, 3).Range.Text = "左小腿夾角(32,35,37)";
                PosteriorTable.Cell(5, 3).Range.Text = "右小腿夾角(33,39,41)";

                //PosteriorTable.Cell(1, 5).Range.Text = "上背與中垂線距離";
                //PosteriorTable.Cell(2, 5).Range.Text = "肩胛骨與中垂線距離";
                //PosteriorTable.Cell(3, 5).Range.Text = "小腿肚與中垂線距離";
                //PosteriorTable.Cell(4, 5).Range.Text = "腳踝與中垂線距離";


                //PosteriorTable.Cell(2, 7).Range.Text = "高度差(左-右)";
                //PosteriorTable.Cell(3, 7).Range.Text = "高度差(左-右)";

                try
                {

                    //灌資料
                    PosteriorTable.Cell(1, 4).Range.Text = Data[21].ToString("f2");//Trunk1
                    PosteriorTable.Cell(2, 4).Range.Text = Data[22].ToString("f2");
                    PosteriorTable.Cell(3, 4).Range.Text = Data[23].ToString("f2");
                    PosteriorTable.Cell(4, 4).Range.Text = Data[19].ToString("f2");//A19
                    PosteriorTable.Cell(5, 4).Range.Text = Data[20].ToString("f2");//A20

                    //PosteriorTable.Cell(1, 6).Range.Text = Data[64].ToString("f2");
                    //PosteriorTable.Cell(2, 6).Range.Text = "左：" + Data[55].ToString("f2") + "右：" + Data[56].ToString("f2");
                    //PosteriorTable.Cell(3, 6).Range.Text = "左：" + Data[58].ToString("f2") + "右：" + Data[59].ToString("f2");
                    //PosteriorTable.Cell(4, 6).Range.Text = "左：" + Data[61].ToString("f2") + "右：" + Data[62].ToString("f2");

                    //PosteriorTable.Cell(2, 8).Range.Text = Data[57].ToString("f2");
                    //PosteriorTable.Cell(3, 8).Range.Text = Data[60].ToString("f2");


                    //【插入圖片】           
                    wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;
                    //if (PosteriorChecked)
                    //    wrdRng.InlineShapes.AddPicture(System.IO.Path.Combine(myDesktopPath, "Posterior.jpg"), ref oMissing, ref oMissing, ref oMissing);
                    
                }
                catch { }

                #endregion
            }

            if (AnteriorChecked || LeftLateralChecked || PosteriorChecked)
            {
                //另存文件
                String fileNameString = "";
                Object oSavePath = System.IO.Path.Combine(myDesktopPath, "EmptyReport.doc");
                if (AnteriorChecked)
                {
                    fileNameString += "Anterior";
                }
                if (LeftLateralChecked)
                {
                    fileNameString += "LeftLateral";
                }
                if (PosteriorChecked)
                {
                    fileNameString += "Posterior";
                }
                fileNameString += ".doc";
                oSavePath = System.IO.Path.Combine(myDesktopPath, fileNameString);

                Object oFormat = Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatDocument;    //格式
                try
                {
                    oDoc.SaveAs(ref oSavePath, ref oFormat,
                             ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                             ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                             ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                             ref oMissing, ref oMissing);
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error! : The word file is already opened");
                }
            }
        }
    }
}
