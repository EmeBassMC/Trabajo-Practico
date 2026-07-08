using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasUnitarias
{
    [TestClass]
    public class ArquitecturaTests
    {
        [TestMethod]
        public void DAL_NoDebeReferenciarBLL()
        {
            var dalAssembly = typeof(DAL.clsPacienteDAL).Assembly;
            var referenciadas = dalAssembly.GetReferencedAssemblies().Select(a => a.Name);
            Assert.IsFalse(referenciadas.Contains("BLL"),
                "DAL no debe depender de BLL: rompería la arquitectura en capas y generaría una dependencia circular (BLL ya depende de DAL).");
        }

        [TestMethod]
        public void DAL_NoDebeReferenciarUI()
        {
            var dalAssembly = typeof(DAL.clsPacienteDAL).Assembly;
            var referenciadas = dalAssembly.GetReferencedAssemblies().Select(a => a.Name);
            Assert.IsFalse(referenciadas.Contains("UI"),
                "DAL no debe depender de UI.");
        }

        [TestMethod]
        public void BE_NoDebeReferenciarNingunaOtraCapa()
        {
            var beAssembly = typeof(BE.clsPacienteBE).Assembly;
            var referenciadas = beAssembly.GetReferencedAssemblies().Select(a => a.Name).ToList();
            Assert.IsFalse(referenciadas.Contains("BLL"), "BE no debe depender de BLL.");
            Assert.IsFalse(referenciadas.Contains("DAL"), "BE no debe depender de DAL.");
            Assert.IsFalse(referenciadas.Contains("UI"), "BE no debe depender de UI.");
        }

        [TestMethod]
        public void BLL_NoDebeReferenciarUI()
        {
            var bllAssembly = typeof(BLL.clsPacienteBLL).Assembly;
            var referenciadas = bllAssembly.GetReferencedAssemblies().Select(a => a.Name);
            Assert.IsFalse(referenciadas.Contains("UI"),
                "BLL no debe depender de UI: la dependencia tiene que ir siempre UI → BLL, nunca al revés.");
        }

        [TestMethod]
        public void BLL_DebeReferenciarDAL()
        {
            var bllAssembly = typeof(BLL.clsPacienteBLL).Assembly;
            var referenciadas = bllAssembly.GetReferencedAssemblies().Select(a => a.Name);
            Assert.IsTrue(referenciadas.Contains("DAL"),
                "BLL sí debe depender de DAL: es el flujo normal de la arquitectura en capas.");
        }
    }
}