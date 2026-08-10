using CalRD.Dusts;
using CalRD.Items.Placeables.Furniture;
using CalRD.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.Astral
{
    public class AstralChestLocked : ModTile
    {
        public override LocalizedText DefaultContainerName(int frameX, int frameY) => ItemLoader.GetItem(ModContent.ItemType<AstralChest>()).GetLocalization("DisplayName");

        public override void SetStaticDefaults()
        {
            this.SetUpChest();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Astral Chest");
            AddMapEntry(new Color(174, 129, 92), name, MapChestName);
            DustType = ModContent.DustType<AstralBasic>();
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Containers };
            TileID.Sets.BasicChest[Type] = true;
            // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AstralChest>();
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

        public override bool IsLockedChest(int i, int j) => Main.tile[i, j].TileFrameX / 36 == 1;

        public override bool UnlockChest(int i, int j, ref short frameXAdjustment, ref int dustType, ref bool manual)
        {
            if (!CalamityWorld.downedAstrageldon)
                return false;

            dustType = this.DustType;

            return true;
        }

        public string MapChestName(string name, int i, int j)
        {
            // Bounds check
            if (i < 0 || i >= Main.maxTilesX || j < 0 || j >= Main.maxTilesY)
                return name;

            // Tile null check
            Tile tile = Main.tile[i, j];
            if (tile == null)
                return name;

            int left = i;
            int top = j;
            if (tile.TileFrameX % 36 != 0)
                left--;
            if (tile.TileFrameY != 0)
                top--;

            int chest = Chest.FindChest(left, top);
            return name + (Main.chest[chest].name != "" ? ": " + Main.chest[chest].name : "");
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 1;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ////Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 32, // ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */);
            Chest.DestroyChest(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            int left = i;
            int top = j;

            if (tile.TileFrameX % 36 != 0)
            {
                left--;
            }
            if (tile.TileFrameY != 0)
            {
                top--;
            }
            return CalamityUtils.LockedChestRightClick(IsLockedChest(left, top), left, top, i, j);
        }

        public override void MouseOver(int i, int j)
        {
            CalamityUtils.ChestMouseOver<AstralChest>("Astral Chest", i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            CalamityUtils.ChestMouseFar<AstralChest>("Astral Chest", i, j);
        }
    }
}
