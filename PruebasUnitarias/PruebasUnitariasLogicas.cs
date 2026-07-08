using BE;
using BLL;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PruebasUnitarias
{
    [TestClass]
    public class LogicaDeNegocioTests
    {
        // ---------- T04: Composite ----------
        [TestMethod]
        public void Composite_GrupoDevuelvePermisosDeSusHijos()
        {
            var hoja1 = new csRolSimple { IdRol = 1, Nombre = "Pacientes.Ver" };
            var hoja2 = new csRolSimple { IdRol = 2, Nombre = "Pacientes.Agregar" };
            var grupo = new csRolGrupo { IdRol = 3, Nombre = "GestionPacientes" };
            grupo.Agregar(hoja1);
            grupo.Agregar(hoja2);

            List<string> permisos = grupo.ObtenerPermisos();

            CollectionAssert.Contains(permisos, "Pacientes.Ver");
            CollectionAssert.Contains(permisos, "Pacientes.Agregar");
        }

        [TestMethod]
        public void Composite_NoExplotaConCicloEntreGrupos()
        {
            // grupoA contiene a grupoB, y grupoB contiene a grupoA: ciclo real
            var grupoA = new csRolGrupo { IdRol = 10, Nombre = "A" };
            var grupoB = new csRolGrupo { IdRol = 11, Nombre = "B" };
            grupoA.Agregar(grupoB);
            grupoB.Agregar(grupoA);

            // Si el guard de ciclos no funcionara, esto tiraría StackOverflowException
            // y el test ni siquiera terminaría de correr.
            List<string> permisos = grupoA.ObtenerPermisos();

            Assert.IsNotNull(permisos);
        }

        [TestMethod]
        public void Composite_HojaSinHijosNoPuedeAgregar()
        {
            var hoja = new csRolSimple { IdRol = 1, Nombre = "Pacientes.Ver" };
            var otraHoja = new csRolSimple { IdRol = 2, Nombre = "Pacientes.Agregar" };

            Assert.ThrowsException<NotSupportedException>(() => hoja.Agregar(otraHoja));
        }

        // ---------- T07/T08: Dígito Verificador ----------
        [TestMethod]
        public void DVH_NoDependeDelIdPersona()
        {
            var paciente1 = new clsPacienteBE
            {
                IdPersona = 0, // como recién creado, antes del INSERT
                Nombre = "Federico",
                Apellido = "Mendez",
                DNI = "41691979",
                Telefono = "1158471020",
                Email = "test@test.com",
                FechaNacimiento = new DateTime(1999, 1, 1),
                ObraSocial = "OSDE"
            };
            var paciente2 = new clsPacienteBE
            {
                IdPersona = 999, // mismo paciente, pero con un Id distinto (ya guardado)
                Nombre = "Federico",
                Apellido = "Mendez",
                DNI = "41691979",
                Telefono = "1158471020",
                Email = "test@test.com",
                FechaNacimiento = new DateTime(1999, 1, 1),
                ObraSocial = "OSDE"
            };

            int dvh1 = clsDigitoVerificador.CalcularDVH(paciente1);
            int dvh2 = clsDigitoVerificador.CalcularDVH(paciente2);

            Assert.AreEqual(dvh1, dvh2,
                "El DVH no debe cambiar según el Id: al insertar, el Id todavía no existe (es 0), y debe coincidir con el DVH recalculado luego con el Id real.");
        }

        [TestMethod]
        public void DVH_CambiaSiCambiaElDNI()
        {
            var pacienteA = new clsPacienteBE { DNI = "41691979", Nombre = "Fede", Apellido = "Mendez", Telefono = "", Email = "", ObraSocial = "" };
            var pacienteB = new clsPacienteBE { DNI = "99999999", Nombre = "Fede", Apellido = "Mendez", Telefono = "", Email = "", ObraSocial = "" };

            int dvhA = clsDigitoVerificador.CalcularDVH(pacienteA);
            int dvhB = clsDigitoVerificador.CalcularDVH(pacienteB);

            Assert.AreNotEqual(dvhA, dvhB,
                "Si el DNI fue manipulado, el DVH tiene que detectarlo (dar un valor distinto).");
        }

        // ---------- T03: Encriptado ----------
        [TestMethod]
        public void Encriptacion_DesencriptarDevuelveElTextoOriginal()
        {
            string original = "federico.mendez@test.com";

            string encriptado = clsEncriptacion.Encriptar(original);
            string desencriptado = clsEncriptacion.Desencriptar(encriptado);

            Assert.AreEqual(original, desencriptado);
            Assert.AreNotEqual(original, encriptado,
                "El texto encriptado no debe ser igual al texto plano.");
        }

        [TestMethod]
        public void Encriptacion_MismoTextoDaResultadosDistintos()
        {
            // Por el IV aleatorio, encriptar el mismo texto dos veces debe dar blobs distintos
            string original = "test@test.com";

            string encriptado1 = clsEncriptacion.Encriptar(original);
            string encriptado2 = clsEncriptacion.Encriptar(original);

            Assert.AreNotEqual(encriptado1, encriptado2,
                "El IV aleatorio debe hacer que cada encriptación sea distinta, aunque el texto sea el mismo.");
        }

        // ---------- T02: Singleton ----------
        [TestMethod]
        public void SesionActual_SiempreDevuelveLaMismaInstancia()
        {
            var instancia1 = clsSesionActual.GetInstancia();
            var instancia2 = clsSesionActual.GetInstancia();

            Assert.AreSame(instancia1, instancia2,
                "clsSesionActual debe ser un Singleton: dos llamadas a GetInstancia() tienen que devolver la misma referencia.");
        }
    }
}