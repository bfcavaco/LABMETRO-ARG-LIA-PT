using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using LabMetro.DataAccessLayer.dlEquipamentoTableAdapters;
using LabMetro.DataAccessLayer; 


//using LabMetro.DataAccessLayer.dlEquipamento;
//estava a tentar fazer algo como no tutorial do Scott Mitchell
//http://www.asp.net/learn/data-access/tutorial-69-cs.aspx
 

namespace LabMetro.BusinessLogicLayer
{
    public class EquipamentoBLL
    {
        //private EmployeesTableAdapter _employeesAdapter = null;
        //protected EmployeesTableAdapter Adapter
        //{
        //    get
        //    {
        //        if (_employeesAdapter == null)
        //            _employeesAdapter = new EmployeesTableAdapter();

        //        return _employeesAdapter;
        //    }
        //}

        //[System.ComponentModel.DataObjectMethodAttribute
        //    (System.ComponentModel.DataObjectMethodType.Select, true)]
        //public NorthwindWithSprocs.EmployeesDataTable GetEmployees()
        //{
        //    return Adapter.GetEmployees();
        //}

        //[System.ComponentModel.DataObjectMethodAttribute
        //    (System.ComponentModel.DataObjectMethodType.Delete, true)]
        //public bool DeleteEmployee(int employeeID)
        //{
        //    int rowsAffected = Adapter.Delete(employeeID);

        //    // Return true if precisely one row was deleted, otherwise false
        //    return rowsAffected == 1;
        //}

//DataObjectMethodAttribute Class

//Identifies a data operation method exposed by a type, what type of operation the method performs, and whether the method is the default data method. This class cannot be inherited. 

//You can use the DataObjectMethodAttribute to identify data operation methods on a type marked with the DataObjectAttribute attribute so that they are more easily identified by callers using reflection. When the DataObjectMethodAttribute attribute is applied to a method, it describes the type of operation the method performs and indicates whether the method is the default data operation method of a type. Components such as the ObjectDataSource control and the ObjectDataSourceDesigner class examine the values of this attribute, if present, to help determine which data method to call at run time. 
        private DMMTableAdapter _DMMTableAdapter = null;


        protected DMMTableAdapter Adapter
        {
            get 
            {

                if (_DMMTableAdapter == null) _DMMTableAdapter = new DMMTableAdapter();

                return _DMMTableAdapter;
            }
        }


        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, true)]
        public dlEquipamento.DMMDataTable GetListaEquipamentos()
        {
            return Adapter.GetData(); 
        }

        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public dlEquipamento.DMMDataTable BLLGetEquipamentoById(int idEquipamento)
        {

            return Adapter.GetDataByIdEquipamento(idEquipamento);
        }


        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
        public dlEquipamento.DMMDataTable GetEquipamentosByIdEmpresa(int idEmpresa)
        {

            return Adapter.GetDataByIdEmpresa(idEmpresa);
        }
      

        //isto é o interno.
        //manda apenas uma vez o idUtilizdor (labmetro, logado) que vai actualizar os campos 
        //e mando apaenas la em baixo directamente, esta funcao nao recebe este id, apenas os valores dos campos.
        //idUtilCriacao, idUtilAlteracao 
        //as datas sao alteradas por getdate no servidor
        [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Insert, true)]
        public int BLLInsertEquipamento(int idEmpresa, int idTipoEquipamento, string numSerie, string numIdentificacao, double? alcanceInf, double? alcanceSup, int? idUnidadeAlcance, string alcance, string resolucao, int? idClasse, string classe, string forma, string fabricante, string refUltimaCalibracao, System.DateTime? dtUltimaCalibracao, short? periodicidadeCalibracao, bool activo, string observacoes, bool? calibInt, bool? bCertConclusivo, string campo1, string campo2, int? idMarca, int? idModelo, string entidadeUltimaCalibracao, int? idEstadoUtilizacao, int? idEstadoEquipamento, System.DateTime? dtEntradaServico, string fornecedor, string responsavelEquipamento, decimal? precoClienteEuros, decimal? custoClienteEuros, string localizacao, string criterios, string pontosCalibracao, string texto, int? idEstadoRelacaoCalibracao, int? idTipoIntervencao) //copiei isto da classview! nao sei como, tem de se carregar em cima da definicao e aparece dps algures numa caixinha esta definicao. e tenho de ter a janela principal object browser aberta
            //objectexplorer, carregar em cima da classe... 

        {
            int rowsAffected = 0;
            DMMTableAdapter ta = new DMMTableAdapter();
            try
            {
                int idUtilizador =  System.Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                rowsAffected = ta.stpInsertDMM(idEmpresa, idTipoEquipamento, numSerie, numIdentificacao, alcanceInf, alcanceSup, idUnidadeAlcance, alcance, resolucao, idClasse, classe, forma, fabricante, refUltimaCalibracao, dtUltimaCalibracao, periodicidadeCalibracao, activo, observacoes, calibInt, bCertConclusivo, campo1, campo2, idMarca, idModelo, entidadeUltimaCalibracao, idEstadoUtilizacao, idEstadoEquipamento, dtEntradaServico, fornecedor, responsavelEquipamento, precoClienteEuros, custoClienteEuros, localizacao, criterios, pontosCalibracao, texto, idEstadoRelacaoCalibracao, idTipoIntervencao, idUtilizador);


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
        //public int UpdEquipamentoCompleto(int idEmpresa, int idTipoEquipamento, string numSerie, string numIdentificacao, double? alcanceInf, double? alcanceSup, int? idUnidadeAlcance, string alcance, string resolucao, string modelo, string marca, int? idClasse, string classe, string forma, string fabricante, string refUltimaCalibracao, System.DateTime? dtUltimaCalibracao, short? periodicidadeCalibracao, bool activo, string observacoes, System.DateTime dtCriacao, int idUtilCriacao, System.DateTime dtAlteracao, int idUtilAlteracao, bool? calibInt, bool? bCertConclusivo, string campo1, string campo2, int? idMarca, int? idModelo, string entidadeUltimaCalibracao, int? idEstadoUtilizacao, string idEstadoEquipamento, System.DateTime? dtEntradaServico, string fornecedor, string responsavelEquipamento, decimal? precoClienteEuros, decimal? custoClienteEuros, string localizacao, string criterios, string pontosCalibracao, string texto, int? idEstadoRelacaoCalibracao, int? idTipoIntervencao, System.Guid? idUtilCriacaoCliente, System.Guid? idUtilAlteracaoCliente, System.DateTime? dtCriacaoCliente, System.DateTime? dtAlteracaoCliente, int idEquipamento)

            //tirei os params de user criacao e user actualização
           
        public int BLLUpdateEquipamento(int idEmpresa, int idTipoEquipamento, string numSerie, string numIdentificacao, double? alcanceInf, double? alcanceSup, int? idUnidadeAlcance, string alcance, string resolucao, int? idClasse, string classe, string forma, string fabricante, string refUltimaCalibracao, System.DateTime? dtUltimaCalibracao, short? periodicidadeCalibracao, bool activo, string observacoes, bool? calibInt, bool? bCertConclusivo, string campo1, string campo2, int? idMarca, int? idModelo, string entidadeUltimaCalibracao, int? idEstadoUtilizacao, int? idEstadoEquipamento, System.DateTime? dtEntradaServico, string fornecedor, string responsavelEquipamento, decimal? precoClienteEuros, decimal? custoClienteEuros, string localizacao, string criterios, string pontosCalibracao, string texto, int? idEstadoRelacaoCalibracao, int? idTipoIntervencao, int idEquipamento)
        {

            int rowsAffected = 0;
            DMMTableAdapter ta = new DMMTableAdapter();

             try
             {

           
                 //rowsAffected = ta.UpdateEquipamentosCompletos(idEmpresa, idTipoEquipamento, numSerie, numIdentificacao, alcanceInf, alcanceSup, idUnidadeAlcance, alcance, resolucao, modelo, marca, idClasse, classe, forma, fabricante, refUltimaCalibracao, dtUltimaCalibracao, periodicidadeCalibracao, activo, observacoes, dtCriacao, idUtilCriacao, dtAlteracao, idUtilAlteracao, calibInt, bCertConclusivo, campo1, campo2, idMarca, idModelo, entidadeUltimaCalibracao, idEstadoUtilizacao, idEstadoEquipamento, dtEntradaServico, fornecedor, responsavelEquipamento, precoClienteEuros, custoClienteEuros, localizacao, criterios, pontosCalibracao, texto, idEstadoRelacaoCalibracao, idTipoIntervencao, idUtilCriacaoCliente, idUtilAlteracaoCliente, dtCriacaoCliente, dtAlteracaoCliente, idEquipamento);

                 DateTime dtAlteracao = DateTime.Now; 
                 int idUtilizador=  System.Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString());
                 rowsAffected = ta.stpUpdateDMM(idEmpresa, idTipoEquipamento, numSerie, numIdentificacao, alcanceInf, alcanceSup, idUnidadeAlcance, alcance, resolucao, idClasse, classe, forma, fabricante, refUltimaCalibracao, dtUltimaCalibracao, periodicidadeCalibracao, activo, observacoes, idUtilizador, calibInt, bCertConclusivo, campo1, campo2, idMarca, idModelo, entidadeUltimaCalibracao, idEstadoUtilizacao, idEstadoEquipamento, dtEntradaServico, fornecedor, responsavelEquipamento, precoClienteEuros, custoClienteEuros, localizacao, criterios, pontosCalibracao, texto, idEstadoRelacaoCalibracao, idTipoIntervencao, idEquipamento);

             }
             catch (Exception)
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
