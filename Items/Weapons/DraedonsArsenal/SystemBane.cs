using CalRD.Items.Materials;
using CalRD.Items.Weapons.Rogue;
using CalRD.Projectiles.DraedonsArsenal;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace CalRD.Items.Weapons.DraedonsArsenal
{
    public class SystemBane : RogueWeapon
    {
        public const int MaxDeployedProjectiles = 5;
        
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("System Bane");
/*
            Tooltip.SetDefault("Can be used to quickly send out an electromagnetic blast, strong enough to target organic nervous systems.\n" +
                               "Hurls an unstable device which sticks to the ground and shocks nearby enemies with lightning\n" +
                               "Stealth strikes make the device emit a large, damaging EMP field");
*/
        }

        public override void SafeSetDefaults()
        {
            CalamityGlobalItem modItem = Item.Calamity();

            Item.damage = 45;
            modItem.rogue = true;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.width = 42;
            Item.height = 36;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = CalamityGlobalItem.Rarity8BuyPrice;
            Item.rare = ItemRarityID.Red;
            modItem.customRarity = CalamityRarity.DraedonRust;
            Item.UseSound = SoundID.Item1;

            Item.shootSpeed = 16f;
            Item.shoot = ModContent.ProjectileType<SystemBaneProjectile>();

            modItem.UsesCharge = true;
            modItem.MaxCharge = 135f;
            modItem.ChargePerUse = 0.085f;
        }

        public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] < MaxDeployedProjectiles;

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectileDirect(Entity.GetSource_FromThis(), position, new Vector2(velocity.X, velocity.Y), type, damage, Item.knockBack, player.whoAmI, 0f, 0f).Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MysteriousCircuitry>(), 20);
            recipe.AddIngredient(ModContent.ItemType<DubiousPlating>(), 10);
            recipe.AddIngredient(ModContent.ItemType<BarofLife>(), 5);
            recipe.AddIngredient(ModContent.ItemType<InfectedArmorPlating>(), 5);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
