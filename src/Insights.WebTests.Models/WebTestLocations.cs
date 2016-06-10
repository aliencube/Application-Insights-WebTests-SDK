using System.Collections.Generic;

using Hyak.Common;

namespace Aliencube.Azure.Insights.WebTests.Models
{
    /// <summary>
    /// This represents the locations entity for web test.
    /// </summary>
    public class WebTestLocations
    {
        /// <summary>
        /// Gets US: IL-Chicago
        /// </summary>
        public static WebTestLocation UsIlChicago = new WebTestLocation("us-il-ch1-azr");

        /// <summary>
        /// Gets US: CA-San Jose
        /// </summary>
        public static WebTestLocation UsCaSanJose = new WebTestLocation("us-ca-sjc-azr");

        /// <summary>
        /// Gets US: TX-San Antonio
        /// </summary>
        public static WebTestLocation UsTxSanAntonio = new WebTestLocation("us-tx-sn1-azr");

        /// <summary>
        /// Gets US: VA-Ashburn
        /// </summary>
        public static WebTestLocation UsVaAshburn = new WebTestLocation("us-va-ash-azr");

        /// <summary>
        /// Gets US: FL-Miami
        /// </summary>
        public static WebTestLocation UsFlMiami = new WebTestLocation("us-fl-mia-edge");

        /// <summary>
        /// Gets SG: Singapore
        /// </summary>
        public static WebTestLocation SgSingapore = new WebTestLocation("apac-sg-sin-azr");

        /// <summary>
        /// Gets SE: Stockholm
        /// </summary>
        public static WebTestLocation SeStockholm = new WebTestLocation("emea-se-sto-edge");

        /// <summary>
        /// Gets RU: Moscow
        /// </summary>
        public static WebTestLocation RuMoscow = new WebTestLocation("emea-ru-msa-edge");

        /// <summary>
        /// Gets NL: Amsterdam
        /// </summary>
        public static WebTestLocation NlAmsterdam = new WebTestLocation("emea-nl-ams-azr");

        /// <summary>
        /// Gets JP: Kawaguchi
        /// </summary>
        public static WebTestLocation JpKawaguchi = new WebTestLocation("apac-jp-kaw-edge");

        /// <summary>
        /// Gets IE: Dublin
        /// </summary>
        public static WebTestLocation IeDublin = new WebTestLocation("emea-gb-db3-azr");

        /// <summary>
        /// Gets HK: Hong Kong
        /// </summary>
        public static WebTestLocation HkHongKong = new WebTestLocation("apac-hk-hkn-azr");

        /// <summary>
        /// Gets FR: Paris
        /// </summary>
        public static WebTestLocation FrParis = new WebTestLocation("emea-fr-pra-edge");

        /// <summary>
        /// Gets CH: Zurich
        /// </summary>
        public static WebTestLocation ChZurich = new WebTestLocation("emea-ch-zrh-edge");

        /// <summary>
        /// Gets BR: Sao Paulo
        /// </summary>
        public static WebTestLocation BrSaoPaulo = new WebTestLocation("latam-br-gru-edge");

        /// <summary>
        /// Gets AU: Sydney
        /// </summary>
        public static WebTestLocation AuSydney = new WebTestLocation("emea-au-syd-edge");

        /// <summary>
        /// Gets the list of <see cref="WebTestLocation"/> objects.
        /// </summary>
        /// <param name="locations"><see cref="TestLocations"/> value.</param>
        /// <returns>Returns the list of the <see cref="WebTestLocation"/> objects.</returns>
        public static IList<WebTestLocation> GetWebTestLocations(TestLocations locations)
        {
            var results = new LazyList<WebTestLocation>();
            if (locations.HasFlag(TestLocations.UsIlChicago))
            {
                results.Add(UsIlChicago);
            }

            if (locations.HasFlag(TestLocations.UsCaSanJose))
            {
                results.Add(UsCaSanJose);
            }

            if (locations.HasFlag(TestLocations.UsTxSanAntonio))
            {
                results.Add(UsTxSanAntonio);
            }

            if (locations.HasFlag(TestLocations.UsVaAshburn))
            {
                results.Add(UsVaAshburn);
            }

            if (locations.HasFlag(TestLocations.UsFlMiami))
            {
                results.Add(UsFlMiami);
            }

            if (locations.HasFlag(TestLocations.SgSingapore))
            {
                results.Add(SgSingapore);
            }

            if (locations.HasFlag(TestLocations.SeStockholm))
            {
                results.Add(SeStockholm);
            }

            if (locations.HasFlag(TestLocations.RuMoscow))
            {
                results.Add(RuMoscow);
            }

            if (locations.HasFlag(TestLocations.NlAmsterdam))
            {
                results.Add(NlAmsterdam);
            }

            if (locations.HasFlag(TestLocations.JpKawaguchi))
            {
                results.Add(JpKawaguchi);
            }

            if (locations.HasFlag(TestLocations.IeDublin))
            {
                results.Add(IeDublin);
            }

            if (locations.HasFlag(TestLocations.HkHongKong))
            {
                results.Add(HkHongKong);
            }

            if (locations.HasFlag(TestLocations.FrParis))
            {
                results.Add(FrParis);
            }

            if (locations.HasFlag(TestLocations.ChZurich))
            {
                results.Add(ChZurich);
            }

            if (locations.HasFlag(TestLocations.BrSaoPaulo))
            {
                results.Add(BrSaoPaulo);
            }

            if (locations.HasFlag(TestLocations.AuSydney))
            {
                results.Add(AuSydney);
            }

            return results;
        }
    }
}