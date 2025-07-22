using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LabMetro.DataAccessLayer.dlIntervencaoTableAdapters;
using LabMetro.DataAccessLayer;


//using LabMetro.DataAccessLayer.dlEquipamento;
//estava a tentar fazer algo como no tutorial do Scott Mitchell
//http://www.asp.net/learn/data-access/tutorial-69-cs.aspx


namespace LabMetro.BusinessLogicLayer
{
    public class IntervencaoBLL
    {
       

        //DataObjectMethodAttribute Class

        //Identifies a data operation method exposed by a type, what type of operation the method performs, and whether the method is the default data method. This class cannot be inherited. 

        //You can use the DataObjectMethodAttribute to identify data operation methods on a type marked with the DataObjectAttribute attribute so that they are more easily identified by callers using reflection. When the DataObjectMethodAttribute attribute is applied to a method, it describes the type of operation the method performs and indicates whether the method is the default data operation method of a type. Components such as the ObjectDataSource control and the ObjectDataSourceDesigner class examine the values of this attribute, if present, to help determine which data method to call at run time. 
        private IntervencaoTableAdapter  _IntervencaoTableAdapterr = null;


        protected IntervencaoTableAdapter Adapter
        {
            get
            {

                if (_IntervencaoTableAdapterr == null) _IntervencaoTableAdapterr = new IntervencaoTableAdapter();

                return _IntervencaoTableAdapterr;
            }
        }


        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public dlIntervencao.IntervencaoDataTable GetListaIntervencoes()
        {
            return Adapter.GetData();
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public dlIntervencao.IntervencaoDataTable BLLGetIntervencaoById(int idIntervencao)
        {

            return Adapter.GetDataByIdIntervencao(idIntervencao);
        }


        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public dlIntervencao.IntervencaoDataTable BLLGetIntervencoesByIdEquipamento(int idEquipamento)
        {

            return Adapter.GetDataByIdEquipamento(idEquipamento);
        }


       
      
        //e mando apenas la em baixo directamente, esta funcao nao recebe este id, apenas os valores dos campos.
        
        //as datas criacao e alteracao sao alteradas por getdate no servidor
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public int BLLInsertIntervencao(int idEquipamento, string intervencao, System.DateTime dataIntervencao, string entidadeIntervencao, string numDocumento, string conclusao, string observacoes, string aprovadoPor, System.DateTime? dtAprovacao) 
            
            //copiei isto da classview! nao sei como, tem de se carregar em cima da definicao e aparece dps algures numa caixinha esta definicao. e tenho de ter a janela principal object browser aberta
        {
            int rowsAffected = 0;
            IntervencaoTableAdapter ta = new IntervencaoTableAdapter();
            try
            {
                string idUtilCliente = ""; 
                try
                {
                    idUtilCliente = "5089bba3-5199-4e64-a817-a7f8f832298b"; //so para testes: 
//                    idUtilCliente = System.Web.HttpContext.Current.Session["UserId"].ToString();
                }
                catch 
                {
                    idUtilCliente = "5089bba3-5199-4e64-a817-a7f8f832298b"; //so para testes: 
                }

                rowsAffected = ta.InsertQuery(idEquipamento, intervencao, dataIntervencao, entidadeIntervencao, numDocumento, conclusao, observacoes, aprovadoPor,dtAprovacao, idUtilCliente);


            }
            catch 
            {

            }
            finally
            {
                ta = null;
            }

            return rowsAffected;


            //este codigo aqui abaixo actualiza a tabela pela row, nao sei se isso funciona quando os default methods automaticos nao estao definidos!


            //// Create a new ProductRow instance
            //dlEquipamento.EquipamentosCompletosDataTable equipamentos = new dlEquipamento.EquipamentosCompletosDataTable();


            //dlEquipamento.EquipamentosCompletosRow eq = equipamentos.NewEquipamentosCompletosRow();


            //eq.idEmpresa = idEmpresa;
            //eq.idTipoEquipamento = idTipoEquipamento;
            //eq.numSerie = numSerie; //validar
            //eq.numIdentificacao = numIdentificacao; //validar

            //eq.alcanceInf = alcanceInf;
            //eq.alcanceSup = alcanceSup;
            //eq.alcance = alcance;
            //eq.idUnidadeAlcance = idUnidadeAlcance;
            //eq.resolucao = resolucao;
            //eq.modelo = modelo;
            //eq.marca = marca;
            //eq.idClasse = idClasse;







            //product.ProductName = productName;
            //if (supplierID == null) product.SetSupplierIDNull();
            //else product.SupplierID = supplierID.Value;
            //if (categoryID == null) product.SetCategoryIDNull();
            //else product.CategoryID = categoryID.Value;
            //if (quantityPerUnit == null) product.SetQuantityPerUnitNull();
            //else product.QuantityPerUnit = quantityPerUnit;
            //if (unitPrice == null) product.SetUnitPriceNull();
            //else product.UnitPrice = unitPrice.Value;
            //if (unitsInStock == null) product.SetUnitsInStockNull();
            //else product.UnitsInStock = unitsInStock.Value;
            //if (unitsOnOrder == null) product.SetUnitsOnOrderNull();
            //else product.UnitsOnOrder = unitsOnOrder.Value;
            //if (reorderLevel == null) product.SetReorderLevelNull();
            //else product.ReorderLevel = reorderLevel.Value;
            //product.Discontinued = discontinued;

            //// Add the new product
            //products.AddProductsRow(product);
            //int rowsAffected = Adapter.Update(products);

            //// Return true if precisely one row was inserted,
            //// otherwise false


            //return rowsAffected == 1;
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Update, true)]

        public int BLLUpdateIntervencao(string intervencao, System.DateTime dataIntervencao, string entidadeIntervencao, string numDocumento, string conclusao, string observacoes, string aprovadoPor, System.DateTime? dtAprovacao,  int idIntervencao)


        {

            int rowsAffected = 0;

            IntervencaoTableAdapter ta = new IntervencaoTableAdapter();

            try
            {

                string idUtilCliente = "";
                try
                {
                    idUtilCliente = "5089bba3-5199-4e64-a817-a7f8f832298b"; //so para testes: 
                    //                    idUtilCliente = System.Web.HttpContext.Current.Session["UserId"].ToString();
                }
                catch
                {
                    idUtilCliente = "5089bba3-5199-4e64-a817-a7f8f832298b"; //so para testes: 
                }


                rowsAffected = ta.UpdateQuery(intervencao, dataIntervencao, entidadeIntervencao, numDocumento, conclusao, observacoes, aprovadoPor, dtAprovacao, idUtilCliente, idIntervencao);

            }
            catch 
            {

            }
            finally
            {
                ta = null;
            }

            return rowsAffected;
        }


    }
}
