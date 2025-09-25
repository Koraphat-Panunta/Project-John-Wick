using UnityEngine;
using System.Collections.Generic;

public class CommandBufferManager
{
    private List<CommandBuffer> commandBuffers = new List<CommandBuffer>();
    public CommandBufferManager() { }
    public void CommandBufferProcess()
    {
        if(commandBuffers.Count <= 0)
            return;

        for (int i = 0; i < commandBuffers.Count; i++) 
        {
            if (this.commandBuffers[i].time <= 0)
                this.commandBuffers.RemoveAt(i);
            else
                this.commandBuffers[i].time -= Time.deltaTime;
        }
    }
    public void AddCommand(string name,float bufferTime)
    {
        for (int i = 0; i < commandBuffers.Count; i++) 
        {
            if (commandBuffers[i].name == name)
            {
                commandBuffers[i].time = bufferTime;
                return;
            }
        }
        commandBuffers.Add(new CommandBuffer(name, bufferTime));
    }
    public void RemoveCommand(string name)
    {
        for (int i = 0; i < commandBuffers.Count; i++)
        {
            if (commandBuffers[i].name == name)
            {
                commandBuffers.RemoveAt(i);
                return;
            }
        }
    }
    public bool TryGetCommand(string commandName)
    {
        for (int i = 0; i < commandBuffers.Count; i++)
        {
            if (commandBuffers[i].name == commandName)
            {
                return true;
            }
        }
     return false;
    }
    protected class CommandBuffer
    {
        public string name { get; private set; }
        public float time;
        public CommandBuffer(string stateName,float time)
        {
            this.name = stateName;
            this.time = time;
        }
      
    }
}
