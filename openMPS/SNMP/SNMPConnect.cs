using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnmpSharpNet;
namespace btg.Zaehlerstand.SNMP
{
    static class SNMPConnect
    {
       public static string start(string ip, string oid)
        {
            string s = "";
            OctetString community = new OctetString("public");
            AgentParameters param = new AgentParameters(community);
            param.Version = SnmpVersion.Ver2;

            IpAddress agent = new IpAddress(ip);

            UdpTarget target = new UdpTarget((IPAddress)agent, 161, 2000, 1);


            Oid rootOid = new Oid(oid);


            Oid lastOid = (Oid)rootOid.Clone();


            Pdu pdu = new Pdu(PduType.GetBulk);
            pdu.NonRepeaters = 0;
            pdu.MaxRepetitions = 5;

            while (lastOid != null)
            {

                if (pdu.RequestId != 0)
                {
                    pdu.RequestId += 1;
                }
                pdu.VbList.Clear();
                pdu.VbList.Add(lastOid);
                SnmpV2Packet result = (SnmpV2Packet)target.Request(pdu, param);
                if (result != null)
                {
                    if (result.Pdu.ErrorStatus != 0)
                    {
                        s += "Error in SNMP reply. Error"+ result.Pdu.ErrorStatus + " index" + result.Pdu.ErrorIndex + "\n";
                        lastOid = null;
                        break;
                    }
                    else
                    {
                        foreach (Vb v in result.Pdu.VbList)
                        {
                            if (rootOid.IsRootOf(v.Oid))
                            {
                                string cmd = "Insert into temp_output (name, value) values ('" + v.Oid.ToString()+ "','"
                                    + v.Value.ToString() +"');";
                                SQLiteConnectionTools.ZaehlerstandConfig.shell(cmd);
                                s += 
                                    v.Oid.ToString() + "(" +
                                    SnmpConstants.GetTypeName(v.Value.Type) + "):" +
                                    v.Value.ToString() + "\n";
                                if (v.Value.Type == SnmpConstants.SMI_ENDOFMIBVIEW)
                                    lastOid = null;
                                else
                                    lastOid = v.Oid;
                            }
                            else
                            {
                                lastOid = null;
                            }
                        }
                    }
                }
                else
                {
                    s += "No response received from SNMP agent.";
                }
            }

            target.Close();
            return s;
        }
    }
}

