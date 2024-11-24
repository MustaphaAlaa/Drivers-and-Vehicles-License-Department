﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Types;

namespace Models.App;

public class LocalDrivingLicenseApplication
{
    [Key] public int Id { get; set; }

    [Required]
    [ForeignKey("ApplicationType")]
    public int ApplicationTypeId { get; set; }

    [Required]
    [ForeignKey("LicenseClass")]
    public int LicenseClassId { get; set; }

    public ApplicationType ApplicationType { get; set; }
    public LicenseClass LicenseClass { get; set; }
}