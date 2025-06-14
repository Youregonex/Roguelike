using UnityEngine;
using Yg.Player;

public abstract class PlayerCharacterComponent : MonoBehaviour
{
    public abstract void InitializeComponent(PlayerCharacter playerCharacter);
    public abstract void SaveComponent(PlayerSaveData playerSaveData);
    public abstract void LoadComponent(PlayerSaveData playerSaveData);    
}
