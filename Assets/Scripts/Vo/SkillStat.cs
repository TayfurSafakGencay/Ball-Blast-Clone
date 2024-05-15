using Enum;

namespace Vo
{
  public struct SkillStat
  {
    public SkillType Key { get; set; }

    public int Level { get; set; }
    
    public int Cost { get; set; }
    
    public float Stat { get; set; }
  }
}