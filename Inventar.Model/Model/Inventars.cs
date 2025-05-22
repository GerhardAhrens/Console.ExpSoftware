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
    using global::Inventar.DatabaseCore;

    [DataTable("TAB_Inventar")]
    public sealed partial class Inventars
    {
        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Name { get; }

        [TableColumn(SQLiteDataType.VarChar, 250)]
        public string Description { get; }

        [TableColumn(SQLiteDataType.VarChar, 250)]
        public string InventarInfo { get; }

        [TableColumn(SQLiteDataType.Integer)]
        public int InventarTyp { get; }

        [TableColumn(SQLiteDataType.Boolean)]
        public bool IsActive { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime GekauftAm { get; }

        [TableColumn(SQLiteDataType.Decimal, 8, 2)]
        public decimal KaufBetrag { get; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string CreatedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string ModifiedBy { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime ModifiedOn { get; set; }
    }
}
