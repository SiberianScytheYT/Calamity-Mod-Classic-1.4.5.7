using Terraria.ModLoader;
namespace CalRD.Walls
{
    public class NavystoneWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            DustType = 96;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = fail ? 1 : 3;
        }
    }
}
