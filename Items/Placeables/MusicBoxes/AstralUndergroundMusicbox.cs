using Terraria.ModLoader;
using Terraria.ID;

namespace CalRD.Items.Placeables.MusicBoxes
{
    public class AstralUndergroundMusicbox : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Music Box (Underground Astral Biome)");
        }

        public override void SetDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false;
            Item.useStyle = 1;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.MusicBoxes.AstralUndergroundMusicboxTile>();
            Item.width = 24;
            Item.height = 24;
            Item.rare = 4;
            Item.value = 100000;
            Item.accessory = true;
        }
    }
}