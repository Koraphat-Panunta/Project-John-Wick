using UnityEngine;

public interface IDebuggedAble 
{
   public T Debugged<T>(IDebugger debugger);
}
