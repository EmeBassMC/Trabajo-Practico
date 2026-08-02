using BE;
using DAL;
using System.Collections.Generic;

namespace BLL
{
    public class clsTraduccionBLL
    {
        private clsTraduccionDAL dal = new clsTraduccionDAL();

        public List<clsTraduccionBE> GetByIdioma(int idIdioma)
        {
            return dal.GetByIdioma(idIdioma);
        }

        public List<string> GetClaves()
        {
            return dal.GetClaves();
        }


        public bool Update(clsTraduccionBE t)
        {
            if (t.IdTraduccion <= 0) return false;
            if (string.IsNullOrEmpty(t.Texto)) return false;
            return dal.Update(t);
        }

        public bool Delete(int idTraduccion)
        {
            if (idTraduccion <= 0) return false;
            return dal.Delete(idTraduccion);
        }
        public int EscanearYGenerarClaves(Dictionary<string, string> claves, Dictionary<string, string> formularios)
        {
            if (claves == null || claves.Count == 0) return 0;
            return dal.EscanearYGenerarClaves(claves, formularios);
        }
    }
}