namespace NorthGomel.ElectrumXClient.IntegrationTests;

internal static class IntegrationTestConstants
{
    public const string Address = "bc1qns9f7yfx3ry9lj6yz7c9er0vwa0ye2eklpzqfw";
    public const string SmallHistoryAddress = "bc1qxhfm2naq6j900g7tzk8dwpxswlqlr7xnu2reg2";

    public const string ConfirmedTransactionHash = "800f58491fa33aec3a9a2d56834519dc0c933e69002eec0a4e57319dbdf19e0c";
    public const string GenesisTransactionHash = "4a5e1e4baab89f3a32518a88c31bc87f618f76673e2cc77ab2127b7afdeda33b";
    public const string MainnetGenesisHash = "000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f";


    public static readonly string ScriptHash = ElectrumXScriptHash.FromBitcoinAddress(Address);
    public static readonly string SmallHistoryScriptHash = ElectrumXScriptHash.FromBitcoinAddress(SmallHistoryAddress);
}
