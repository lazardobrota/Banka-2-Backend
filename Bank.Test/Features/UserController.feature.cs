﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace Bank.Test2.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("User controller testing")]
    [NUnit.Framework.FixtureLifeCycleAttribute(NUnit.Framework.LifeCycle.InstancePerTestCase)]
    public partial class UserControllerTestingFeature
    {
        
        private global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private static global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "User controller testing", null, global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
        
#line 1 "UserController.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(featureHint: featureInfo);
            if (((testRunner.FeatureContext != null) 
                        && (testRunner.FeatureContext.FeatureInfo.Equals(featureInfo) == false)))
            {
                await testRunner.OnFeatureEndAsync();
            }
            if ((testRunner.FeatureContext == null))
            {
                await testRunner.OnFeatureStartAsync(featureInfo);
            }
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
            global::Reqnroll.TestRunnerManager.ReleaseTestRunner(testRunner);
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<NUnit.Framework.TestContext>(NUnit.Framework.TestContext.CurrentContext);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find user by ID")]
        public async System.Threading.Tasks.Task FindUserByID()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Find user by ID", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 3
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 4
        await testRunner.GivenAsync("user with ID \"123e4567-e89b-12d3-a456-426614174000\" exists in database", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 5
        await testRunner.WhenAsync("I request user with ID \"123e4567-e89b-12d3-a456-426614174000\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 6
        await testRunner.ThenAsync("the response should contain user details", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Find user by non-existent ID")]
        public async System.Threading.Tasks.Task FindUserByNon_ExistentID()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Find user by non-existent ID", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 8
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 9
        await testRunner.WhenAsync("I request user with ID \"00000000-0000-0000-0000-000000000000\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 10
        await testRunner.ThenAsync("the response should be Not Found", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Fetch all users successfully")]
        public async System.Threading.Tasks.Task FetchAllUsersSuccessfully()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Fetch all users successfully", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 12
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 13
        await testRunner.WhenAsync("I request all users with no filters", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 14
        await testRunner.ThenAsync("the response should contain at least 1 user", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Login attempt with a non-existent email")]
        public async System.Threading.Tasks.Task LoginAttemptWithANon_ExistentEmail()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Login attempt with a non-existent email", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 22
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 23
        await testRunner.WhenAsync("I send a login request with email \"invalidd@example.com\" and password \"Password12" +
                        "3\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 24
        await testRunner.ThenAsync("the response should return status 404 Not Found", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 25
        await testRunner.AndAsync("the response should contain the message \"User with the specified email address wa" +
                        "s not found.\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Login attempt with an incorrect password")]
        public async System.Threading.Tasks.Task LoginAttemptWithAnIncorrectPassword()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Login attempt with an incorrect password", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 27
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 28
        await testRunner.GivenAsync("a user exists with email \"user@example.com\" and password \"Password123\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 29
        await testRunner.WhenAsync("I send a login request with email \"user@example.com\" and password \"WrongPassword\"" +
                        "", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 30
        await testRunner.ThenAsync("the response should return status 400 Bad Request", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 31
        await testRunner.ThenAsync("the response should contain the message: The password is incorrect.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("User activates account successfully")]
        public async System.Threading.Tasks.Task UserActivatesAccountSuccessfully()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("User activates account successfully", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 33
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 34
        await testRunner.GivenAsync("a valid activation token for user \"user@example.com\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 35
        await testRunner.AndAsync("a password \"SecurePass123\" and confirmation password \"SecurePass123\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 36
        await testRunner.WhenAsync("I send an activation request with the token and passwords", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 37
        await testRunner.ThenAsync("the response should return status 202 Accepted", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Activation fails due to invalid token")]
        public async System.Threading.Tasks.Task ActivationFailsDueToInvalidToken()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Activation fails due to invalid token", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 39
    this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 40
        await testRunner.GivenAsync("an invalid activation token", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 41
        await testRunner.AndAsync("a password \"SecurePass123\" and confirmation password \"SecurePass123\"", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
#line 42
        await testRunner.WhenAsync("I attempt to activate the account using the invalid token", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 43
        await testRunner.ThenAsync("the activation response should return status 400 Bad Request", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
#line 44
        await testRunner.AndAsync("the response should contain the message: Invalid token", ((string)(null)), ((global::Reqnroll.Table)(null)), "And ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
    }
}
#pragma warning restore
#endregion
