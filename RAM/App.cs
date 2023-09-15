using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Windows.Media;

namespace RAM
{
    internal class App : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            application.CreateRibbonTab("RAM");

            //Создание категории ""
            RibbonPanel panel = application.CreateRibbonPanel("RAM", "Общая");

            PushButtonData pbdFillNames = new PushButtonData("Заполнить фамилии", "Заполнить\nштамп", assemblyPath, "RAM.FileName.CommandFillNames");

            Image img2 = Properties.Resources.img2;
            ImageSource imgLarge2 = GetImageSourse(img2);

            pbdFillNames.LargeImage = imgLarge2;
            panel.AddItem(pbdFillNames);

            PushButtonData pbdCreateDetailView = new PushButtonData("Получить виды", "Получить\nвиды", assemblyPath, "RAM.CreateDetailView.CreateDetailViewCommand");

            Image img3 = Properties.Resources.img3;
            ImageSource imgLarge3 = GetImageSourse(img3);

            pbdCreateDetailView.LargeImage = imgLarge3;
            panel.AddItem(pbdCreateDetailView);


            PushButtonData pbdHideSchedule = new PushButtonData("Подчистить ВРС", "Подчистить\nВРС", assemblyPath, "RAM.HideScheduleColumns.CommandHide");

            Image img4 = Properties.Resources.img4;
            ImageSource imgLarge4 = GetImageSourse(img4);
            pbdHideSchedule.LargeImage = imgLarge4;
            panel.AddItem(pbdHideSchedule);

            PushButtonData pbdRevitLink = new PushButtonData("Выгрузить ссылки", "Выгрузка\nссылок", assemblyPath, "RAM.RevitLink.CommandRevitLink");

            Image img5 = Properties.Resources.img5;
            ImageSource imgLarge5 = GetImageSourse(img5);
            pbdRevitLink.LargeImage = imgLarge5;
            panel.AddItem(pbdRevitLink);

            PushButtonData pbdGetElement = new PushButtonData("Получить элементы", "Получить\nэлементы", assemblyPath, "RAM.GetElement.CommandGetElement");

            //Image img5 = Properties.Resources.img5;
            //ImageSource imgLarge5 = GetImageSourse(img5);
            //pbdRevitLink.LargeImage = imgLarge5;
            panel.AddItem(pbdGetElement);


            return Result.Succeeded;
        }
        private BitmapSource GetImageSourse(Image img)
        {
            BitmapImage bmp = new BitmapImage();
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                ms.Position = 0;

                bmp.BeginInit();

                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.UriSource = null;
                bmp.StreamSource = ms;

                bmp.EndInit();
            }
            return bmp;
        }

    }
}
