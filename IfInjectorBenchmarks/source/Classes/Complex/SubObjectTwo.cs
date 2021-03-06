﻿using System;
using System.ComponentModel.Composition;

namespace IocPerformance.Classes.Complex
{
    [IfInjector.ImplementedBy(typeof(SubObjectTwo))]
    public interface ISubObjectTwo
    {
    }

    [Export(typeof(ISubObjectTwo)), PartCreationPolicy(CreationPolicy.NonShared)]
    public class SubObjectTwo : ISubObjectTwo
    {
        [ImportingConstructor]
        public SubObjectTwo(ISecondService secondService)
        {
            if (secondService == null)
            {
                throw new ArgumentNullException("secondService");
            }
        }
    }
}
