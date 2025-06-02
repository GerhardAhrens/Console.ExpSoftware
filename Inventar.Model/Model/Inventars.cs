//-----------------------------------------------------------------------
// <copyright file="Inventar.cs" company="Lifeprojects.de">
//     Class: Inventar
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>22.05.2025 08:36:58</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Model
{
    using System;
    using System.ComponentModel;

    using Inventar.Generator;

    [DataTable("TAB_Inventar")]
    public sealed partial class Inventars
    {
        private DateTime _GekauftAm;

        public Inventars()
        {
        }
        public Inventars(string name, DateTime gekauftAm, decimal kaufBetrag)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.KaufBetrag = kaufBetrag;
            this.IsActive = true;
            this.GekauftAm = gekauftAm;
            this.InventarAlter = this.GetAge(gekauftAm);
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Name { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 250)]
        public string Description { get; set; }

        [TableColumn(SQLiteDataType.VarChar, 250)]
        public string InventarInfo { get; set; }

        [ColumnIndex("InventarTyp","InvTyp",ListSortDirection.Ascending)]
        [TableColumn(SQLiteDataType.Integer)]
        public int InventarTyp { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Ablageort { get; set; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsActive { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime GekauftAm
        {
            get { return this._GekauftAm; }
            set
            { 
                this._GekauftAm = value;
                this.InventarAlter = this.GetAge(value);
            }
        }


        [TableColumn(SQLiteDataType.Integer)]
        public int InventarAlter { get; private set; }

        [TableColumn(SQLiteDataType.Decimal, 8, 2)]
        public decimal KaufBetrag { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string ModifiedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        private int GetAge(DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dateOfBirth.Year * 100 + dateOfBirth.Month) * 100 + dateOfBirth.Day;

            return (a - b) / 10000;
        }
    }
}
