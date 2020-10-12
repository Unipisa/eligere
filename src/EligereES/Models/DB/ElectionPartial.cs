using EligereES.Models.Extensions;
using System;
using System.Collections.Generic;

namespace EligereES.Models.DB
{
    public partial class Election
    {
        public ElectionConfiguration ElectionConfiguration
        {
            get { return ElectionConfiguration.FromJson(Configuration); }
        }
    }
}
