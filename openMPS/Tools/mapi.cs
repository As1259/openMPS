#region Copyright

// Copyright (c) 2018, Andreas Schreiner

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace de.fearvel.openMPS.Tools
{
    internal class MAPI
    {
        private const int MAPI_LOGON_UI = 0x00000001;
        private const int MAPI_DIALOG = 0x00000008;
        private const int maxAttachments = 20;

        /// <summary>
        ///     The errors
        /// </summary>
        private readonly string[] errors =
        {
            "OK [0]", "User abort [1]", "General MAPI failure [2]",
            "MAPI login failure [3]", "Disk full [4]",
            "Insufficient memory [5]", "Access denied [6]",
            "-unknown- [7]", "Too many sessions [8]",
            "Too many files were specified [9]",
            "Too many recipients were specified [10]",
            "A specified attachment was not found [11]",
            "Attachment open failure [12]",
            "Attachment write failure [13]", "Unknown recipient [14]",
            "Bad recipient type [15]", "No messages [16]",
            "Invalid message [17]", "Text too large [18]",
            "Invalid session [19]", "Type not supported [20]",
            "A recipient was specified ambiguously [21]",
            "Message in use [22]", "Network failure [23]",
            "Invalid edit fields [24]", "Invalid recipients [25]",
            "Not supported [26]"
        };

        private readonly List<string> m_attachments = new List<string>();


        /// <summary>
        ///     The m recipients
        /// </summary>
        private readonly List<MapiRecipDesc> m_recipients = new
            List<MapiRecipDesc>();

        private int m_lastError;

        /// <summary>
        ///     Adds the recipient to.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public bool AddRecipientTo(string email)
        {
            return AddRecipient(email, HowTo.MAPI_TO);
        }

        /// <summary>
        ///     Adds the recipient cc.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public bool AddRecipientCC(string email)
        {
            return AddRecipient(email, HowTo.MAPI_TO);
        }

        /// <summary>
        ///     Adds the recipient BCC.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public bool AddRecipientBCC(string email)
        {
            return AddRecipient(email, HowTo.MAPI_TO);
        }

        /// <summary>
        ///     Adds the attachment.
        /// </summary>
        /// <param name="strAttachmentFileName">Name of the string attachment file.</param>
        public void AddAttachment(string strAttachmentFileName)
        {
            m_attachments.Add(strAttachmentFileName);
        }

        /// <summary>
        ///     Sends the mail popup.
        /// </summary>
        /// <param name="strSubject">The string subject.</param>
        /// <param name="strBody">The string body.</param>
        /// <returns></returns>
        public int SendMailPopup(string strSubject, string strBody)
        {
            return SendMail(strSubject, strBody, MAPI_LOGON_UI | MAPI_DIALOG);
        }

        /// <summary>
        ///     Sends the mail direct.
        /// </summary>
        /// <param name="strSubject">The string subject.</param>
        /// <param name="strBody">The string body.</param>
        /// <returns></returns>
        public int SendMailDirect(string strSubject, string strBody)
        {
            return SendMail(strSubject, strBody, MAPI_LOGON_UI);
        }


        /// <summary>
        ///     Mapis the send mail.
        /// </summary>
        /// <param name="sess">The sess.</param>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="message">The message.</param>
        /// <param name="flg">The FLG.</param>
        /// <param name="rsv">The RSV.</param>
        /// <returns></returns>
        [DllImport("MAPI32.DLL")]
        private static extern int MAPISendMail(IntPtr sess, IntPtr hwnd,
            MapiMessage message, int flg, int rsv);

        /// <summary>
        ///     Sends the mail.
        /// </summary>
        /// <param name="strSubject">The string subject.</param>
        /// <param name="strBody">The string body.</param>
        /// <param name="how">The how.</param>
        /// <returns></returns>
        private int SendMail(string strSubject, string strBody, int how)
        {
            var msg = new MapiMessage
            {
                subject = strSubject,
                noteText = strBody
            };

            msg.recips = GetRecipients(out msg.recipCount);
            msg.files = GetAttachments(out msg.fileCount);

            m_lastError = MAPISendMail(new IntPtr(0), new IntPtr(0), msg, how,
                0);
            if (m_lastError > 1)
                throw new ArgumentException();

            Cleanup(ref msg);
            return m_lastError;
        }

        /// <summary>
        ///     Adds the recipient.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="howTo">The how to.</param>
        /// <returns></returns>
        private bool AddRecipient(string email, HowTo howTo)
        {
            var recipient = new MapiRecipDesc
            {
                recipClass = (int)howTo,
                name = email
            };
            m_recipients.Add(recipient);
            return true;
        }

        /// <summary>
        ///     Gets the recipients.
        /// </summary>
        /// <param name="recipCount">The recip count.</param>
        /// <returns></returns>
        private IntPtr GetRecipients(out int recipCount)
        {
            recipCount = 0;
            if (m_recipients.Count == 0)
                return IntPtr.Zero;

            var size = Marshal.SizeOf(typeof(MapiRecipDesc));
            var intPtr = Marshal.AllocHGlobal(m_recipients.Count * size);

            var ptr = (int) intPtr;
            foreach (var mapiDesc in m_recipients)
            {
                Marshal.StructureToPtr(mapiDesc, (IntPtr) ptr, false);
                ptr += size;
            }

            recipCount = m_recipients.Count;
            return intPtr;
        }

        /// <summary>
        ///     Gets the attachments.
        /// </summary>
        /// <param name="fileCount">The file count.</param>
        /// <returns></returns>
        private IntPtr GetAttachments(out int fileCount)
        {
            fileCount = 0;
            if (m_attachments == null)
                return IntPtr.Zero;
            if (m_attachments.Count <= 0 || m_attachments.Count >
                maxAttachments)
                return IntPtr.Zero;
            var size = Marshal.SizeOf(typeof(MapiFileDesc));
            var intPtr = Marshal.AllocHGlobal(m_attachments.Count * size);
            var mapiFileDesc = new MapiFileDesc
            {
                position = -1
            };
            var ptr = (int) intPtr;
            foreach (var strAttachment in m_attachments)
            {
                mapiFileDesc.name = Path.GetFileName(strAttachment);
                mapiFileDesc.path = strAttachment;
                Marshal.StructureToPtr(mapiFileDesc, (IntPtr) ptr, false);
                ptr += size;
            }

            fileCount = m_attachments.Count;
            return intPtr;
        }

        /// <summary>
        ///     Cleanups the specified MSG.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void Cleanup(ref MapiMessage msg)
        {
            var size = Marshal.SizeOf(typeof(MapiRecipDesc));
            var ptr = 0;
            if (msg.recips != IntPtr.Zero)
            {
                ptr = (int) msg.recips;
                for (var i = 0; i < msg.recipCount; i++)
                {
                    Marshal.DestroyStructure((IntPtr) ptr,
                        typeof(MapiRecipDesc));
                    ptr += size;
                }

                Marshal.FreeHGlobal(msg.recips);
            }

            if (msg.files != IntPtr.Zero)
            {
                size = Marshal.SizeOf(typeof(MapiFileDesc));

                ptr = (int) msg.files;
                for (var i = 0; i < msg.fileCount; i++)
                {
                    Marshal.DestroyStructure((IntPtr) ptr,
                        typeof(MapiFileDesc));
                    ptr += size;
                }

                Marshal.FreeHGlobal(msg.files);
            }

            m_recipients.Clear();
            m_attachments.Clear();
            m_lastError = 0;
        }

        /// <summary>
        ///     Gets the last error.
        /// </summary>
        /// <returns></returns>
        public string GetLastError()
        {
            if (m_lastError <= 26)
                return errors[m_lastError];
            return "MAPI error [" + m_lastError + "]";
        }

        private enum HowTo
        {
            MAPI_ORIG = 0,
            MAPI_TO,
            MAPI_CC,
            MAPI_BCC
        }
    }

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiMessage
    {
        public string conversationID;
        public string dateReceived;
        public int fileCount;
        public IntPtr files;
        public int flags;
        public string messageType;
        public string noteText;
        public IntPtr originator;
        public int recipCount;
        public IntPtr recips;
        public int reserved;
        public string subject;
    }

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiFileDesc
    {
        public int flags;
        public string name;
        public string path;
        public int position;
        public int reserved;
        public IntPtr type;
    }

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public class MapiRecipDesc
    {
        public string address;
        public int eIDSize;
        public IntPtr entryID;
        public string name;
        public int recipClass;
        public int reserved;
    }
}