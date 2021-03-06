﻿using System.Data.SqlClient;

namespace Csharp_ORM_Example
{
    public class SellersRepository : Repository
    {
        protected SellersRepository(string table_name, string primary_key) : base(table_name, primary_key)
        {
        }

        private static SellersRepository instance;
        public static SellersRepository getInstance()
        {
            if (SellersRepository.instance == null)
                SellersRepository.instance = new SellersRepository("sellers", "id");

            return SellersRepository.instance;
        }

        public Seller createNewEntity()
        {
            string query = "INSERT INTO " + table_name +
                " (name)" +
                "VALUES ('')";

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }

            Seller seller = (Seller)this.getList()
                .setOrder(primary_key, Order.DESC)
                .setLimit(1)
                .fetch()[0];

            return seller;
        }

        internal override void updateEntity(Entity entity)
        {
            if (entity.GetType() != typeof(Seller))
                throw new System.Exception("Cannot update type " + entity.GetType());

            Seller seller = (Seller)entity;

            string query = "UPDATE " + table_name +
                " SET name = '" + seller.Name +
                "' WHERE " + this.primary_key + " = " + entity.Id;

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        internal override Entity[] getEntities(string query)
        {
            return getEntities<Seller>(query);
        }
    }
}
