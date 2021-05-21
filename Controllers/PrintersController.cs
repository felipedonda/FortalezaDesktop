using System;
using System.Collections.Generic;
using System.Text;
using FortalezaDesktop.Models;
using System.Windows;
using System.Windows.Controls;
using System.Printing;

namespace FortalezaDesktop.Controllers
{
    public class PrintersController
    {
        public static bool PrintCupom(Page page)
        {
            PrintDialog printDialog = new PrintDialog();
            if (string.IsNullOrEmpty(UserPreferences.Preferences.ImpressoraCupom.ImpressoraPadrao))
            {
                    
                printDialog.ShowDialog();
                    
            }
            else
            {
                printDialog.PrintQueue = new PrintQueue(new PrintServer(), UserPreferences.Preferences.ImpressoraCupom.ImpressoraPadrao);
            }
            printDialog.PrintVisual(page, "");
            return true;
        }

        public static bool PrintCozinha(Page page)
        {
            PrintDialog printDialog = new PrintDialog();
            if (string.IsNullOrEmpty(UserPreferences.Preferences.ImpressoraCozinha.ImpressoraPadrao))
            {

                printDialog.ShowDialog();

            }
            else
            {
                printDialog.PrintQueue = new PrintQueue(new PrintServer(), UserPreferences.Preferences.ImpressoraCozinha.ImpressoraPadrao);
            }
            printDialog.PrintVisual(page, "");
            return true;
        }

        public static bool PrintRelatorio(Page page)
        {
            PrintDialog printDialog = new PrintDialog();
            if (string.IsNullOrEmpty(UserPreferences.Preferences.ImpressoraRelatorio.ImpressoraPadrao))
            {

                printDialog.ShowDialog();

            }
            else
            {
                printDialog.PrintQueue = new PrintQueue(new PrintServer(), UserPreferences.Preferences.ImpressoraRelatorio.ImpressoraPadrao);
            }
            printDialog.PrintVisual(page, "");
            return true;
        }
    }
}
