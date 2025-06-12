using System;
using UnityEngine;

public interface IHeardingAble 
{
   public void GotHearding(INoiseMakingAble noiseMaker);
    public Action<INoiseMakingAble> NotifyGotHearing { get; set; }
}
