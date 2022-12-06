using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Messages;
using Nop.Core.Events;
using Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Domain;
using Nop.Services.Localization;
using Nop.Services.Messages;

namespace Nop.Plugin.Widgets.BSEmployee.Areas.Admin.Services
{
    public class EmployeeEmailService : IEmployeeEmailService
    {
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly ILocalizationService _localizationService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly EmailAccountSettings _emailAccountSettings;

        public EmployeeEmailService(IStoreContext storeContext,
                                    ILanguageService languageService,
                                    IMessageTokenProvider messageTokenProvider,
                                    IEventPublisher eventPublisher,
                                    IWorkflowMessageService workflowMessageService,
                                    IMessageTemplateService messageTemplateService,
                                    ILocalizationService localizationService,
                                    IEmailAccountService emailAccountService,
                                    EmailAccountSettings emailAccountSettings)
        {
            _storeContext = storeContext;
            _languageService = languageService;
            _messageTokenProvider = messageTokenProvider;
            _eventPublisher = eventPublisher;
            _workflowMessageService = workflowMessageService;
            _messageTemplateService = messageTemplateService;
            _localizationService = localizationService;
            _emailAccountService = emailAccountService;
            _emailAccountSettings = emailAccountSettings;
        }

        public async Task<IList<int>> SendBSEmployeeCreateNotificationAsync(Employee employee, int languageId)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            var store = await _storeContext.GetCurrentStoreAsync();
            languageId = await EnsureLanguageIsActiveAsync(languageId, store.Id);

            var messageTemplates = await GetActiveMessageTemplatesAsync("BSEmployeeSendEmail", store.Id);
            if (!messageTemplates.Any())
                return new List<int>();

            //tokens
            var commonTokens = new List<Token>();


            return await messageTemplates.SelectAwait(async messageTemplate =>
            {
                //email account
                var emailAccount = await GetEmailAccountOfMessageTemplateAsync(messageTemplate, languageId);

                var tokens = new List<Token>(commonTokens);
                await _messageTokenProvider.AddStoreTokensAsync(tokens, store, emailAccount);

                //event notification
                await _eventPublisher.MessageTokensAddedAsync(messageTemplate, tokens);

                //var toEmail = emailAccount.Email;
                var toEmail = "busohag@gmail.com";
                //var toName = emailAccount.DisplayName;
                var toName = "Readul Islam Sohag";

                return await _workflowMessageService.SendNotificationAsync(messageTemplate, emailAccount, languageId, tokens, toEmail, toName);
            }).ToListAsync();
        }

        protected virtual async Task<int> EnsureLanguageIsActiveAsync(int languageId, int storeId)
        {
            //load language by specified ID
            var language = await _languageService.GetLanguageByIdAsync(languageId);

            if (language == null || !language.Published)
            {
                //load any language from the specified store
                language = (await _languageService.GetAllLanguagesAsync(storeId: storeId)).FirstOrDefault();
            }

            if (language == null || !language.Published)
            {
                //load any language
                language = (await _languageService.GetAllLanguagesAsync()).FirstOrDefault();
            }

            if (language == null)
                throw new Exception("No active language could be loaded");

            return language.Id;
        }

        protected virtual async Task<IList<MessageTemplate>> GetActiveMessageTemplatesAsync(string messageTemplateName, int storeId)
        {
            //get message templates by the name
            var messageTemplates = await _messageTemplateService.GetMessageTemplatesByNameAsync(messageTemplateName, storeId);

            //no template found
            if (!messageTemplates?.Any() ?? true)
                return new List<MessageTemplate>();

            //filter active templates
            messageTemplates = messageTemplates.Where(messageTemplate => messageTemplate.IsActive).ToList();

            return messageTemplates;
        }

        protected virtual async Task<EmailAccount> GetEmailAccountOfMessageTemplateAsync(MessageTemplate messageTemplate, int languageId)
        {
            var emailAccountId = await _localizationService.GetLocalizedAsync(messageTemplate, mt => mt.EmailAccountId, languageId);
            //some 0 validation (for localizable "Email account" dropdownlist which saves 0 if "Standard" value is chosen)
            if (emailAccountId == 0)
                emailAccountId = messageTemplate.EmailAccountId;

            var emailAccount = (await _emailAccountService.GetEmailAccountByIdAsync(emailAccountId) ?? await _emailAccountService.GetEmailAccountByIdAsync(_emailAccountSettings.DefaultEmailAccountId)) ??
                               (await _emailAccountService.GetAllEmailAccountsAsync()).FirstOrDefault();
            return emailAccount;
        }

    }
}
