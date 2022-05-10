using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using MvcDynamoAWS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcDynamoAWS.Services
{
    public class ServiceDynamoDB
    {
        private DynamoDBContext context;
        private AmazonDynamoDBClient client;
        public ServiceDynamoDB()
        {
            //A PARTIR DE UN CLIENTE DYNAMODB SE CREA UN CONTEXT
            this.client = new AmazonDynamoDBClient();
            this.context = new DynamoDBContext(client);
        }

        public async Task CreateCocheAsync(Coche car)
        {
            await this.context.SaveAsync<Coche>(car);
        }

        public async Task DeleteCocheAsync(int idCoche)
        {
            await this.context.DeleteAsync<Coche>(idCoche);
        }

        public async Task<Coche> FindCocheAsync(int idCoche)
        {
            //SI ESTAMOS BUSCANDO POR SU PARTITION KEY (PRIMARY KEY)
            //TENEMOS UN METODO PARA CARGAR SU BUSQUEDA
            return await this.context.LoadAsync<Coche>(idCoche);
        }

        public async Task<List<Coche>> GetCochesAsync()
        {
            //LO PRIMERO ES RECUPERAR LA TABLA DE LOS OBJETOS
            var tabla = this.context.GetTargetTable<Coche>();
            //PARA RECUPERAR TODOS O PARA BUSCAR, DEBEMOS INDICARLO 
            //CON UN OBJETO ScanOptions
            var scanOptions = new ScanOperationConfig();
            var results = tabla.Scan(scanOptions);
            //LO QUE DEVUELVE EN EL MOMENTO DE BUSCAR
            //SON OBJETOS DE LA CLASE Document
            List<Document> data = await results.GetNextSetAsync();
            //DEBEMOS CONVERTIR LOS OBJETOS Document A NUESTRO TIPO
            var cars =
    this.context.FromDocuments<Coche>(data);
            return cars.ToList();
        }

        public async Task<List<Coche>> SearchCochesAsync(string query)
        {
            List<ScanCondition> conditions = new List<ScanCondition>();

            conditions.Add(new ScanCondition("marca", ScanOperator.Equal, query));

            var cars = await this.context.ScanAsync<Coche>(conditions, null).GetRemainingAsync();

            return cars.ToList();
        }
    }

}
