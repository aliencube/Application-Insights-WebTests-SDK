using System;

namespace Aliencube.Azure.Insights.WebTests.Models.Options
{
    /// <summary>
    /// This specifies the web test locations.
    /// </summary>
    [Flags]
    public enum TestLocations
    {
        /// <summary>
        /// Identifies none selected.
        /// </summary>
        None = 0,

        /// <summary>
        /// Identifies US: IL - Chicago.
        /// </summary>
        UsIlChicago = 1 << 0,

        /// <summary>
        /// Identifies US: CA - San Jose.
        /// </summary>
        UsCaSanJose = 1 << 1,

        /// <summary>
        /// Identifies US: TX - San Antonio.
        /// </summary>
        UsTxSanAntonio = 1 << 2,

        /// <summary>
        /// Identifies US: VS - Ashburn.
        /// </summary>
        UsVaAshburn = 1 << 3,

        /// <summary>
        /// Identifies US: FL - Miami.
        /// </summary>
        UsFlMiami = 1 << 4,

        /// <summary>
        /// Identifies SG: Singapore.
        /// </summary>
        SgSingapore = 1 << 5,

        /// <summary>
        /// Identifies SE: Stockholm.
        /// </summary>
        SeStockholm = 1 << 6,

        /// <summary>
        /// Identifies RU: Moscow.
        /// </summary>
        RuMoscow = 1 << 7,

        /// <summary>
        /// Identifies NL: Amsterdam.
        /// </summary>
        NlAmsterdam = 1 << 8,

        /// <summary>
        /// Identifies JP: Kawaguchi.
        /// </summary>
        JpKawaguchi = 1 << 9,

        /// <summary>
        /// Identifies IE: Dublin.
        /// </summary>
        IeDublin = 1 << 10,

        /// <summary>
        /// Identifies HK: Hong Kong.
        /// </summary>
        HkHongKong = 1 << 11,

        /// <summary>
        /// Identifies FR: Paris.
        /// </summary>
        FrParis = 1 << 12,

        /// <summary>
        /// Identifies CH: Zurich.
        /// </summary>
        ChZurich = 1 << 13,

        /// <summary>
        /// Identifies BR: Sao Paulo.
        /// </summary>
        BrSaoPaulo = 1 << 14,

        /// <summary>
        /// Identifies AU: Sydney.
        /// </summary>
        AuSydney = 1 << 15,
    }
}