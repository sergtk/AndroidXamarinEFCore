using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Db.Entities.Remote
{
    public class MachineModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// If true, the use `Name` to access specific virtual machine model.
        /// Please avoid to user `Id` directly as predefined value.
        /// </summary>
        public bool IsVirtual { get; set; }
    }
}
