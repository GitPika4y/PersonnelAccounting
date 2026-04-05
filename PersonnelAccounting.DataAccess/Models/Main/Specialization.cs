using System.ComponentModel.DataAnnotations;

namespace Data.Models.Main;

public enum Specialization
{
    [Display(Name = "Программная инженерия")]
    SoftwareEngineering,

    [Display(Name = "Информационные системы и технологии")]
    InformationSystems,

    [Display(Name = "Прикладная информатика")]
    AppliedInformatics,

    [Display(Name = "Кибербезопасность")]
    CyberSecurity,

    [Display(Name = "Экономика")]
    Economics,

    [Display(Name = "Бухгалтерский учет и аудит")]
    Accounting,

    [Display(Name = "Финансы и кредит")]
    Finance,

    [Display(Name = "Менеджмент")]
    Management,

    [Display(Name = "Управление персоналом")]
    HumanResources,

    [Display(Name = "Маркетинг")]
    Marketing,

    [Display(Name = "Юриспруденция")]
    Law,

    [Display(Name = "Государственное и муниципальное управление")]
    PublicAdministration,

    [Display(Name = "Логистика")]
    Logistics,

    [Display(Name = "Строительство")]
    CivilEngineering,

    [Display(Name = "Электроэнергетика")]
    ElectricalEngineering
}