using CalRD.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class DaedalusBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Daedalus Breastplate");
/*
            Tooltip.SetDefault("3% increased damage and critical strike chance");
*/
        }

        public override void SetDefaults()
        {
            Item.width = 18;
            Item.height = 18;
            Item.value = Item.buyPrice(0, 20, 0, 0);
            Item.rare = 5;
            Item.defense = 19; //41
        }

        public override void Load()
        {
            if (Main.netMode != NetmodeID.Server)
                EquipLoader.AddEquipTexture(Mod, Texture + "_Waist", EquipType.Waist, this);
        }
                
        public override void EquipFrameEffects(Player player, EquipType type)
        { 
            if (player.body == Item.bodySlot)
                player.waist = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Waist);
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Generic) += 0.03f;
            player.Calamity().AllCritBoost(3);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VerstaltiteBar>(), 15);
			recipe.AddIngredient(ItemID.CrystalShard, 12);
			recipe.AddIngredient(ModContent.ItemType<EssenceofEleum>(), 3);
			recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
