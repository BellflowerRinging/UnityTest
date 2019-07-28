using System;

public interface IUnit
{
    IUnitAttritubeContorl AttrContorl { get; }
}

public interface IUnitAttritubeContorl
{
    IAttritubeMes GetProp(string mes_name);
    void SetProp(IAttritubeMes mes);
}

public class UnitAttritubeContorl : IUnitAttritubeContorl
{
    public IAttritubeMes GetProp(string mes_name)
    {
        throw new NotImplementedException();
    }

    public void SetProp(IAttritubeMes mes)
    {
        throw new NotImplementedException();
    }
}

public interface IAttritubeMes : IMessage
{

}

public interface IAttacker  // 可进行攻击的对象
{
    IUnitAttritubeContorl AttrContorl { get; }
    void Attack();
}

public interface IDamager  // 可受到伤害的对象
{
    IUnitAttritubeContorl AttrContorl { get; }
    void OnDamager();
}

public struct IAttackMes : IMessage
{
    public IAttacker Attacker;
    public float Attack_Value;

    public bool IsEmpty()
    {
        return false;
    }
}

public struct IDamagerMes : IMessage
{
    public IDamager Damager;
    public bool IsEmpty()
    {
        return false;
    }
}

public class DameageManager
{

    public void DoDameage(IAttackMes attack, IDamagerMes damager)
    {
        
    }
}