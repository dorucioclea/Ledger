﻿using SendGrid.Helpers.Mail;
using System.Collections.Generic;

namespace Ledger.CrossCutting.EmailService.Models
{
    public abstract class EmailTemplate
    {
        public EmailAddress To { get; private set; }
        public string TemplateId { get; private set; }

        private Dictionary<string, string> _sendGridSubstitutions;
        public IReadOnlyDictionary<string, string> SendGridSubstitutions
        {
            get
            {
                return _sendGridSubstitutions;
            }
        }

        public EmailTemplate(string to, string templateId)
        {
            To = new EmailAddress(to);
            TemplateId = templateId;

            _sendGridSubstitutions = new Dictionary<string, string>();
        }

        public void AddSendGridSubstitution(string key, string value)
        {
            if (_sendGridSubstitutions.ContainsKey(key))
                _sendGridSubstitutions[key] = value;
            else
                _sendGridSubstitutions.Add(key, value);
        }
    }
}