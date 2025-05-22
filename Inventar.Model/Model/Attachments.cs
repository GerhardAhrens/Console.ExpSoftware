//-----------------------------------------------------------------------
// <copyright file="Attachment.cs" company="www.lifeprojects.de">
//     Class: Attachment
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>30.06.2022 13:53:09</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Inventar.Model
{
    using System;

    using Inventar.DatabaseCore;

    [DataTable("TAB_Attachment")]
    public sealed partial class Attachments
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EffortProject"/> class.
        /// </summary>
        public Attachments()
        {
            this.Id = Guid.NewGuid();
            this.CreatedBy = UserInfo.TS().CurrentUser;
            this.CreatedOn = UserInfo.TS().CurrentTime;
        }

        [PrimaryKey]
        [TableColumn(SQLiteDataType.Guid)]
        public Guid Id { get; set; }

        [TableColumn(SQLiteDataType.Guid)]
        public Guid ObjectId { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string ObjectName { get; set; }

        [TableColumn(SQLiteDataType.BLOB)]
        public byte[] Content { get; set; }

        [TableColumn(SQLiteDataType.Text, 50)]
        public string Filename { get; set; }

        [TableColumn(SQLiteDataType.Text, 10)]
        public string FileExtension { get; set; }

        [TableColumn(SQLiteDataType.DateTime)]
        public DateTime FileDateTime { get; set; }

        [TableColumn(SQLiteDataType.Integer)]
        public long FileSize { get; set; }

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
