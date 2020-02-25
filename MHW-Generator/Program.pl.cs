﻿using System.Collections.Generic;
using MHW_Template;

namespace MHW_Generator {
    public static partial class Program {
        private static void GenCommonPl() {
            GenPlItemParam();
            GenPlPlayerParam();
        }

        private static void GenPlPlayerParam() {
            ushort i = 1;
            ushort j = 1;
            ushort k = 1;
            ushort l = 1;
            ushort m = 1;

            GeneratePlDataProps("MHW_Editor.PlData", "PlPlayerParam", new MhwStructData { // .plp
                size = 20640,
                offsetInitial = 0,
                entryCountOffset = -1,
                uniqueIdFormula = "0",
                encryptionKey = EncryptionKeys.PL_PARAM_KEY,
                entries = new List<MhwStructData.Entry> {
                    new MhwStructData.Entry($"Unk{i++}", 8, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 12, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 16, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 20, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 24, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 28, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 32, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 36, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 40, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 44, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 48, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 52, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 56, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 60, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 64, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 68, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 72, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 76, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 80, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 84, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 88, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 92, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 96, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 100, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 104, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 108, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 112, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 116, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 120, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 124, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 128, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 132, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 136, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 140, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 144, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 148, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 152, typeof(uint)),
                    new MhwStructData.Entry($"Unk{i++}", 156, typeof(uint)),
                    new MhwStructData.Entry($"Unk{i++}", 160, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 164, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 168, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 172, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 176, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 180, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 184, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 188, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 192, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 196, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 200, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 204, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 208, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 212, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 216, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 220, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 224, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 228, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 232, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 236, typeof(byte)),
                    new MhwStructData.Entry($"Unk{i++}", 237, typeof(ushort)),
                    new MhwStructData.Entry($"Unk{i++}", 239, typeof(ushort)),
                    new MhwStructData.Entry($"Unk{i++}", 241, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 245, typeof(byte)),
                    new MhwStructData.Entry($"Unk{i++}", 246, typeof(ushort)),
                    new MhwStructData.Entry($"Unk{i++}", 248, typeof(ushort)),
                    new MhwStructData.Entry($"Unk{i++}", 250, typeof(ushort)),
                    new MhwStructData.Entry($"Unk{i++}", 252, typeof(float)),

                    new MhwStructData.Entry("------Skipping ahead 1.", 256, typeof(byte)),

                    new MhwStructData.Entry("Health Initial Value", 408, typeof(float)),
                    new MhwStructData.Entry("Health Max Value", 412, typeof(float)),
                    new MhwStructData.Entry("Health Damage Recovery Rate", 416, typeof(float)),
                    new MhwStructData.Entry("Health Damage Recovery Wait Time", 420, typeof(float)),
                    new MhwStructData.Entry("Health Damage Recovery Interval", 424, typeof(float)),
                    new MhwStructData.Entry("Health Damage Recovery Value", 428, typeof(float)),

                    new MhwStructData.Entry("Stamina Initial Value", 432, typeof(float)),
                    new MhwStructData.Entry("Stamina Max Value", 436, typeof(float)),
                    new MhwStructData.Entry("Stamina Min Value", 440, typeof(float)),
                    new MhwStructData.Entry("Stamina Tired Value", 444, typeof(float)),
                    new MhwStructData.Entry("Stamina Auto Recover", 448, typeof(float)),
                    new MhwStructData.Entry("Stamina Auto Max Reduce", 452, typeof(float)),
                    new MhwStructData.Entry("Stamina Auto Max Reduce Time", 456, typeof(float)),
                    new MhwStructData.Entry("Stamina IB Unknown", 460, typeof(float)),
                    new MhwStructData.Entry("Stamina Escape Dash Rate", 464, typeof(float)),
                    new MhwStructData.Entry("Stamina Out of Battle Rate", 468, typeof(float)),
                    new MhwStructData.Entry("Stamina Reduce Rate Limit Trigger", 472, typeof(float)),
                    new MhwStructData.Entry("Stamina Reduce Rate Limit Time", 476, typeof(float)),
                    new MhwStructData.Entry("Stamina Mount Endurance Rate", 480, typeof(float)),

                    new MhwStructData.Entry("Stamina Consumption: Dodge", 484, typeof(float)),
                    new MhwStructData.Entry("Stamina Consumption: LS Counter", 488, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 492, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 496, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 500, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 504, typeof(float)),
                    new MhwStructData.Entry("Stamina Consumption: Bow Shoot", 508, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 512, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 516, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 520, typeof(float)),
                    new MhwStructData.Entry("Stamina Consumption: Bow Charge Step", 524, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 528, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: IB Unk{j++}", 532, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: IB Unk{j++}", 536, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: IB Unk{j++}", 540, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: IB Unk{j++}", 544, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 548, typeof(float)),
                    new MhwStructData.Entry($"Stamina Consumption: Unk{j++}", 552, typeof(float)),

                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 556, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 560, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 564, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 568, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 572, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 576, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 580, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 584, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 588, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 592, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 596, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 600, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 604, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 608, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 612, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 616, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 620, typeof(float)),
                    new MhwStructData.Entry($"Stamina Time Reduce mCore: Unk{k++}", 624, typeof(float)),

                    new MhwStructData.Entry($"Mount Reduce Stamina mCore: Unk{l++}", 628, typeof(float)),
                    new MhwStructData.Entry($"Mount Reduce Stamina mCore: Unk{l++}", 632, typeof(float)),

                    new MhwStructData.Entry($"Mount Life Reduce Stamina mCore: Unk{m++}", 636, typeof(float)),
                    new MhwStructData.Entry($"Mount Life Reduce Stamina mCore: Unk{m++}", 640, typeof(float)),
                    new MhwStructData.Entry($"Mount Life Reduce Stamina mCore: Unk{m++}", 644, typeof(float)),
                    new MhwStructData.Entry($"Mount Life Reduce Stamina mCore: Unk{m++}", 648, typeof(float)),

                    new MhwStructData.Entry($"Unk{i++}", 652, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 656, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 660, typeof(float)),
                    new MhwStructData.Entry("Explosive HR Fixed Attack Rate", 664, typeof(float)),
                    new MhwStructData.Entry("Explosive MR Fixed Attack Rate", 668, typeof(float)),
                    new MhwStructData.Entry("Critical Attack Rate", 672, typeof(float)),
                    new MhwStructData.Entry("Bad Critical Attack Rate", 676, typeof(float)),

                    new MhwStructData.Entry("Sharpness Recoil Reduction %", 680, typeof(byte)),
                    new MhwStructData.Entry("Sharpness Recoil Reduction Value", 681, typeof(byte)),
                    new MhwStructData.Entry($"Unk{i++}", 682, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 686, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 690, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 694, typeof(float)),
                    new MhwStructData.Entry($"Unk{i++}", 698, typeof(uint)),
                    new MhwStructData.Entry("Physical Attack Rate Limit", 702, typeof(float)),
                    new MhwStructData.Entry("Elemental Attack Rate Limit", 706, typeof(float)),
                    new MhwStructData.Entry("Condition Attack Flat Limit", 710, typeof(float)),
                    new MhwStructData.Entry("Bowgun Elemental Attack Rate Limit", 714, typeof(float)),
                    new MhwStructData.Entry("Condition Attack Rate Limit", 718, typeof(float)),
                    new MhwStructData.Entry("Stun Attack Rate Limit", 722, typeof(float)),
                    new MhwStructData.Entry("Stamina Attack Rate Limit", 726, typeof(float)),
                    new MhwStructData.Entry("Mount Attack Rate Limit", 730, typeof(float)),
                    new MhwStructData.Entry("Super Armor Stun Damage Rate", 734, typeof(float)),
                    new MhwStructData.Entry("Hyper Armor Damage Rate", 738, typeof(float)),
                    new MhwStructData.Entry("Hyper Armor Stun Damage Rate", 742, typeof(float)),
                    new MhwStructData.Entry("Gunner Defense Rate", 746, typeof(float)),
                    new MhwStructData.Entry("Gunner Elemental Resistance Bonus", 750, typeof(byte)),

                    new MhwStructData.Entry("------Skipping ahead 2.", 751, typeof(float))
                }
            });
        }

        private static void GenPlItemParam() {
            GeneratePlDataProps("MHW_Editor.PlData", "PlItemParam", new MhwStructData { // .plip
                size = 512,
                offsetInitial = 0,
                entryCountOffset = -1,
                uniqueIdFormula = "0",
                encryptionKey = EncryptionKeys.PL_PARAM_KEY,
                entries = new List<MhwStructData.Entry> {
                    new MhwStructData.Entry("Powder Radius", 8, typeof(float)),
                    new MhwStructData.Entry("Potion Power", 12, typeof(uint)),
                    new MhwStructData.Entry("Unk2", 16, typeof(float)),
                    new MhwStructData.Entry("Unk3", 20, typeof(float)),
                    new MhwStructData.Entry("Unk4", 24, typeof(float)),
                    new MhwStructData.Entry("Mega Potion Power", 28, typeof(uint)),
                    new MhwStructData.Entry("Unk5", 32, typeof(float)),
                    new MhwStructData.Entry("Unk6", 36, typeof(float)),
                    new MhwStructData.Entry("Unk7", 40, typeof(float)),
                    new MhwStructData.Entry("Nutrients Power", 44, typeof(byte)),
                    new MhwStructData.Entry("Mega Nutrients Power", 45, typeof(byte)),
                    new MhwStructData.Entry("Unk9", 46, typeof(byte)),
                    new MhwStructData.Entry("Mega Dash Juice Stamina Power", 47, typeof(float)),
                    new MhwStructData.Entry("Dash Juice Stamina Power", 51, typeof(float)),
                    new MhwStructData.Entry("Both Dash Juice Duration", 55, typeof(ushort)),
                    new MhwStructData.Entry("Both Dash Juice Power", 57, typeof(byte)),
                    new MhwStructData.Entry("Immunizer Power", 58, typeof(float)),
                    new MhwStructData.Entry("Immunizer Duration", 62, typeof(ushort)),
                    new MhwStructData.Entry("Astera Jerky Power", 64, typeof(float)),
                    new MhwStructData.Entry("Astera Jerky Duration", 68, typeof(ushort)),
                    new MhwStructData.Entry("Herbal Medicine Power", 70, typeof(ushort)),
                    new MhwStructData.Entry("Sushifish Scale Power", 72, typeof(uint)),
                    new MhwStructData.Entry("Great Sushifish Scale Power", 76, typeof(uint)),
                    new MhwStructData.Entry("Great Sushifish Scale Duration", 80, typeof(float)),
                    new MhwStructData.Entry("Cool Drink Duration", 84, typeof(ushort)),
                    new MhwStructData.Entry("Hot Drink Duration", 86, typeof(ushort)),
                    new MhwStructData.Entry("Might Seed Duration", 88, typeof(ushort)),
                    new MhwStructData.Entry("Might Seed Power", 90, typeof(ushort)),
                    new MhwStructData.Entry("Might Pill Duration", 92, typeof(ushort)),
                    new MhwStructData.Entry("Might Pill Power", 94, typeof(ushort)),
                    new MhwStructData.Entry("Adamant Seed Duration", 96, typeof(ushort)),
                    new MhwStructData.Entry("Adamant Seed Power", 98, typeof(ushort)),
                    new MhwStructData.Entry("Adamant Pill Duration", 100, typeof(ushort)),
                    new MhwStructData.Entry("Adamant Pill Power", 102, typeof(float)),
                    new MhwStructData.Entry("Demondrug Power", 106, typeof(byte)),
                    new MhwStructData.Entry("Mega Demondrug Power", 107, typeof(byte)),
                    new MhwStructData.Entry("Armorskin Power", 108, typeof(byte)),
                    new MhwStructData.Entry("Mega Armorskin Power", 109, typeof(byte)),
                    new MhwStructData.Entry("Unk16", 110, typeof(ushort)),
                    new MhwStructData.Entry("Lifepowder Power", 112, typeof(ushort)),
                    new MhwStructData.Entry("Dust of Life Power", 114, typeof(ushort)),
                    new MhwStructData.Entry("Herbal Powder Power", 116, typeof(ushort)),
                    new MhwStructData.Entry("Demon Powder Power", 118, typeof(ushort)),
                    new MhwStructData.Entry("Demon Powder Duration", 120, typeof(ushort)),
                    new MhwStructData.Entry("Hardshell Powder Power", 122, typeof(ushort)),
                    new MhwStructData.Entry("Hardshell Powder Duration", 124, typeof(ushort)),
                    new MhwStructData.Entry("Demon Ammo Power", 126, typeof(ushort)),
                    new MhwStructData.Entry("Demon Ammo Duration", 128, typeof(ushort)),
                    new MhwStructData.Entry("Armor Ammo Power", 130, typeof(ushort)),
                    new MhwStructData.Entry("Armor Ammo Duration", 132, typeof(ushort)),
                    new MhwStructData.Entry("Ration Power", 134, typeof(ushort)),
                    new MhwStructData.Entry("Well Done Steak Multiplier", 136, typeof(uint)),
                    new MhwStructData.Entry("Well Done Steak Stamina Bonus", 140, typeof(float)),
                    new MhwStructData.Entry("Unk23", 144, typeof(float)),
                    new MhwStructData.Entry("Unk24", 148, typeof(float)),
                    new MhwStructData.Entry("Burnt Meat Stamina Recovery", 152, typeof(ushort)),
                    new MhwStructData.Entry("Unk26", 154, typeof(short)),
                    new MhwStructData.Entry("Unk27", 156, typeof(byte)),
                    new MhwStructData.Entry("Unk28", 157, typeof(float)),
                    new MhwStructData.Entry("Unk29", 161, typeof(float)),
                    new MhwStructData.Entry("Unk30", 165, typeof(float)),
                    new MhwStructData.Entry("Unk31", 169, typeof(float)),
                    new MhwStructData.Entry("Unk32", 173, typeof(float)),
                    new MhwStructData.Entry("Unk33", 177, typeof(float)),
                    new MhwStructData.Entry("Unk34", 181, typeof(float)),
                    new MhwStructData.Entry("Unk35", 185, typeof(float)),
                    new MhwStructData.Entry("Unk36", 189, typeof(float)),
                    new MhwStructData.Entry("Unk37", 193, typeof(float)),
                    new MhwStructData.Entry("Unk38", 197, typeof(float)),
                    new MhwStructData.Entry("Whetstone Sharpness Restored", 201, typeof(uint)),
                    new MhwStructData.Entry("Whetstone Cycles", 205, typeof(uint)),
                    new MhwStructData.Entry("Whetfish Scale Sharpness Restored", 209, typeof(uint)),
                    new MhwStructData.Entry("Whetfish Scale Cycles", 213, typeof(byte)),
                    new MhwStructData.Entry("Whetfish Scale Plus Cycles", 214, typeof(byte)),
                    new MhwStructData.Entry("Whetfish Scale Consume %", 215, typeof(byte)),
                    new MhwStructData.Entry("Powertalon Power", 216, typeof(byte)),
                    new MhwStructData.Entry("Powercharm Power", 217, typeof(byte)),
                    new MhwStructData.Entry("Armortalon Power", 218, typeof(byte)),
                    new MhwStructData.Entry("Armorcharm Power", 219, typeof(byte)),
                    new MhwStructData.Entry("Unk49", 220, typeof(byte)),
                    new MhwStructData.Entry("Unk50", 221, typeof(float)),
                    new MhwStructData.Entry("Unk51", 225, typeof(float)),
                    new MhwStructData.Entry("Unk52", 229, typeof(float)),
                    new MhwStructData.Entry("Unk53", 233, typeof(float)),
                    new MhwStructData.Entry("Unk54", 237, typeof(float)),
                    new MhwStructData.Entry("Unk55", 241, typeof(float)),
                    new MhwStructData.Entry("Unk56", 245, typeof(float)),
                    new MhwStructData.Entry("Unk57", 249, typeof(float)),
                    new MhwStructData.Entry("Unk58", 253, typeof(float)),
                    new MhwStructData.Entry("Unk59", 257, typeof(float)),
                    new MhwStructData.Entry("Unk60", 261, typeof(float)),
                    new MhwStructData.Entry("Unk61", 265, typeof(float)),
                    new MhwStructData.Entry("Unk62", 269, typeof(float)),
                    new MhwStructData.Entry("Unk63", 273, typeof(float)),
                    new MhwStructData.Entry("Unk64", 277, typeof(float)),
                    new MhwStructData.Entry("Unk65", 281, typeof(float)),
                    new MhwStructData.Entry("Unk66", 285, typeof(float)),
                    new MhwStructData.Entry("Unk67", 289, typeof(float)),
                    new MhwStructData.Entry("Unk68", 293, typeof(float)),
                    new MhwStructData.Entry("Unk69", 297, typeof(float)),
                    new MhwStructData.Entry("Unk70", 301, typeof(float)),
                    new MhwStructData.Entry("Unk71", 305, typeof(float)),
                    new MhwStructData.Entry("Unk72", 309, typeof(float)),
                    new MhwStructData.Entry("Unk73", 313, typeof(float)),
                    new MhwStructData.Entry("Unk74", 317, typeof(float)),
                    new MhwStructData.Entry("Unk75", 321, typeof(float)),
                    new MhwStructData.Entry("Unk76", 325, typeof(float)),
                    new MhwStructData.Entry("Unk77", 329, typeof(float)),
                    new MhwStructData.Entry("Unk78", 337, typeof(float)),
                    new MhwStructData.Entry("Unk79", 341, typeof(float)),
                    new MhwStructData.Entry("Unk80", 345, typeof(float)),
                    new MhwStructData.Entry("Unk81", 349, typeof(float)),
                    new MhwStructData.Entry("Unk82", 353, typeof(float)),
                    new MhwStructData.Entry("Unk83", 357, typeof(float)),
                    new MhwStructData.Entry("Unk84", 361, typeof(float)),
                    new MhwStructData.Entry("Unk85", 365, typeof(float)),
                    new MhwStructData.Entry("Unk86", 369, typeof(float)),
                    new MhwStructData.Entry("Unk87", 373, typeof(float)),
                    new MhwStructData.Entry("Unk88", 377, typeof(float)),
                    new MhwStructData.Entry("Unk89", 381, typeof(float)),
                    new MhwStructData.Entry("Unk90", 385, typeof(float)),
                    new MhwStructData.Entry("Unk91", 389, typeof(float)),
                    new MhwStructData.Entry("Unk92", 393, typeof(float)),
                    new MhwStructData.Entry("Unk93", 397, typeof(float)),
                    new MhwStructData.Entry("Unk94", 401, typeof(float)),
                    new MhwStructData.Entry("Unk95", 405, typeof(float)),
                    new MhwStructData.Entry("Unk96", 409, typeof(float)),
                    new MhwStructData.Entry("Unk97", 413, typeof(float)),
                    new MhwStructData.Entry("Unk98", 417, typeof(float)),
                    new MhwStructData.Entry("Unk99", 417, typeof(float)),
                    new MhwStructData.Entry("Unk100", 421, typeof(float)),
                    new MhwStructData.Entry("Unk101", 425, typeof(float)),
                    new MhwStructData.Entry("Unk102", 429, typeof(float)),
                    new MhwStructData.Entry("Unk103", 433, typeof(float)),
                    new MhwStructData.Entry("Unk104", 437, typeof(float)),
                    new MhwStructData.Entry("Unk105", 441, typeof(float)),
                    new MhwStructData.Entry("Deathcream: 455-499 are fishing rumble", 445, typeof(byte)),
                    new MhwStructData.Entry("and skipped for now.", 446, typeof(uint)),
                    new MhwStructData.Entry("Unk106", 500, typeof(float)),
                    new MhwStructData.Entry("Unk107", 504, typeof(float)),
                    new MhwStructData.Entry("Unk108", 508, typeof(float))
                }
            });
        }

        public static void GeneratePlDataProps(string @namespace, string className, MhwStructData structData) {
            GenerateItemProps(@namespace, className, structData);

            WriteResult($"{Global.GENERATED_ROOT}\\{@namespace.Replace(".", "\\")}", @namespace, $"{className}Internal", new PlDataItemTemplate {
                Session = new Dictionary<string, object> {
                    {"_namespace", @namespace},
                    {"className", className},
                    {"structData", structData}
                }
            });
        }
    }
}