
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

    public class Funciones
    {
        public static string conexionMySQL = "server=46.24.8.196;port=3307;uid=joseluis;pwd=UBU_tfg_23_24;database=TFG_UBU";
    }
}
