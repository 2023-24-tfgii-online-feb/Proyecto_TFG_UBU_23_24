﻿
using System.Reflection;

namespace InverIoT
{
    // Optimiza el dgv, usado en (frmResumen) y (frmSeleccion)
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }

    internal class Funciones
    {
    }
}
