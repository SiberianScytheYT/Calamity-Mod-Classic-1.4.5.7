using CalRD.Items.Placeables.Furniture;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CalRD.Tiles.Abyss
{
    public class RustyChestTile : ModTile
    {
        public override LocalizedText DefaultContainerName(int frameX, int frameY) => ItemLoader.GetItem(ModContent.ItemType<RustyChest>()).GetLocalization("DisplayName");

        public override void SetStaticDefaults()
        {
            this.SetUpChest();
            LocalizedText name = CreateMapEntryName();
            // name.SetDefault("Rusty Chest");
            AddMapEntry(new Color(191, 142, 111), name, MapChestName);
            
            TileID.Sets.DisableSmartCursor[Type] = true;
            AdjTiles = new int[] { TileID.Containers };
            TileID.Sets.BasicChest[Type] = true;
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            Dust.NewDust(new Vector2(i, j) * 16f, 16, 16, 180, 0f, 0f, 1, new Color(255, 255, 255), 1f);
            return false;
        }

        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

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
            if (chest == -1)
                return name;
            return name + (Main.chest[chest].name != "" ? ": " + Main.chest[chest].name : "");
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Chest.DestroyChest(i, j);
        }

        public override bool RightClick(int i, int j)
        {
            return CalamityUtils.ChestRightClick(i, j);
        }

        public override void MouseOver(int i, int j)
        {
            CalamityUtils.ChestMouseOver<RustyChest>("Rusty Chest", i, j);
        }

        public override void MouseOverFar(int i, int j)
        {
            CalamityUtils.ChestMouseFar<RustyChest>("Rusty Chest", i, j);
        }
    }
}
