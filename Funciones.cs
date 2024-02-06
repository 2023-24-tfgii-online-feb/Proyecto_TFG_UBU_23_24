
using System.Reflection;

namespace InverIoT
{
    // Optimiza el dgv
    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }

    // Variable con la cadena de conexión al servidor mySQL
    public class Funciones
    {
        public static string conexionMySQL = "server=46.24.8.196;port=3307;uid=joseluis;pwd=UBU_tfg_23_24;database=TFG_UBU";
    }
}
