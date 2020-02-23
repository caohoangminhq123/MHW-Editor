﻿using MHW_Editor.Models;

namespace MHW_Editor.Weapons {
    public partial class LimitBreakMaterialSkill : MhwItem {
        public LimitBreakMaterialSkill(byte[] bytes, ulong offset) : base(bytes, offset) {
        }

        public override string Name => "None";

        [SortOrder(0)]
        public ulong Index => (Offset - InitialOffset) / StructSize;
    }
}