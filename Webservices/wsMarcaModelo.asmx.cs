using System;
    using System.Web;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Web.Services;
    using System.Web.Services.Protocols;
    using AjaxControlToolkit;
    using System.Data;
    using System.Data.SqlClient;


namespace LabMetro.Webservices
{
    /// <summary>
    /// Summary description for wsMarcaModelo
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService()]
    //[System.Web.Script.Services.ScriptService]

    [System.ComponentModel.ToolboxItem(false)]
    public class wsMarcaModelo : System.Web.Services.WebService
    {

     
        [WebMethod]
        public  CascadingDropDownNameValue[] GetMarcas(string knownCategoryValues,string category)
        {
            LabMetro.DataAccessLayer.MarcaModeloTableAdapters.MarcasTableAdapter marcaAdapter = new LabMetro.DataAccessLayer.MarcaModeloTableAdapters.MarcasTableAdapter();

            LabMetro.DataAccessLayer.MarcaModelo.MarcasDataTable marcas = marcaAdapter.GetData();

            List<CascadingDropDownNameValue> values =  new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in marcas)
            {
                string marca = (string)dr["Marca"];
                int idMarca = (int)dr["idMarca"];
                values.Add(new CascadingDropDownNameValue(marca, idMarca.ToString()));
            }
            return values.ToArray();
        }

        [WebMethod]
        public  CascadingDropDownNameValue[] GetModelosByMarca(string knownCategoryValues,string category)
        {
            StringDictionary kv = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues);
            
            int idMarca;
            
            if (!kv.ContainsKey("Marca") || !Int32.TryParse(kv["Marca"], out idMarca))
            {
                return null;
            }

            LabMetro.DataAccessLayer.MarcaModeloTableAdapters.ModelosTableAdapter modeloAdapter =  new LabMetro.DataAccessLayer.MarcaModeloTableAdapters.ModelosTableAdapter();

            LabMetro.DataAccessLayer.MarcaModelo.ModelosDataTable modelos = modeloAdapter.GetDataByIdMarca(idMarca);

            List<CascadingDropDownNameValue> values =  new List<CascadingDropDownNameValue>();
            foreach (DataRow dr in modelos)
            {
                values.Add(new CascadingDropDownNameValue((string)dr["Modelo"], dr["idModelo"].ToString()));
            }
            return values.ToArray();
        }

        // Returns strongly typed (using Generics) collection of ListItems filtered by category, 
        // and paged using a startrow and page size index (both of these are automatically
        // calculated by the GridView).
        //

        [WebMethod]
        public List<System.Web.UI.WebControls.ListItem> ListaModelosPorMarca(int idMarca)
        {

            // List<string> empresas = new List<string>();
            List<System.Web.UI.WebControls.ListItem> modelos = new List<System.Web.UI.WebControls.ListItem>();

            DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter adapter = new LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter();

            DataAccessLayer.dlEquipamento.ModeloDDDataTable results = adapter.GetDataByIdMarca(idMarca);


            foreach (DataAccessLayer.dlEquipamento.ModeloDDRow row in results)
            {
                modelos.Add(new System.Web.UI.WebControls.ListItem(row.idModelo.ToString(), row.descricao));
            }

            return modelos;
        }

        [WebMethod]
        public DataTable wsDTModelo(int idMarca)
        {


            
            DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter adapter = new LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.ModeloDDTableAdapter();

            DataAccessLayer.dlEquipamento.ModeloDDDataTable results = adapter.GetDataByIdMarca(idMarca);


            return results;
            
        }


        [WebMethod]
        public DataTable wsDTMarca()
        {



            DataAccessLayer.dlEquipamentoTableAdapters.MarcaDDTableAdapter adapter = new LabMetro.DataAccessLayer.dlEquipamentoTableAdapters.MarcaDDTableAdapter();

            DataAccessLayer.dlEquipamento.MarcaDDDataTable results = adapter.GetData();


            return results;

        }


    }
}



