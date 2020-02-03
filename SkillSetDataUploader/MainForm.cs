using Agilisium.TalentManager.Dto;
using Agilisium.TalentManager.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkillSetDataUploader
{
    public partial class MainForm : Form
    {
        private Agilisium.TalentManager.Model.TalentManagerDataContext sqlDataContext = new Agilisium.TalentManager.Model.TalentManagerDataContext();
        private Agilisium.TalentManager.PostgresModel.TalentManagerDataContext postgressDataContext = new Agilisium.TalentManager.PostgresModel.TalentManagerDataContext();
        private Npgsql.NpgsqlConnection con = null;
        private EmployeeTechRepository repository = null;

        private List<SkillsetData> rawData = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void BrowseButtonClick(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                DialogResult res = dlg.ShowDialog();
                dlg.Title = "Select Raw Skillset CSV File";
                dlg.InitialDirectory = @"c:\";
                dlg.Filter = "All files (*.csv)|*.csv";
                if (res != DialogResult.OK)
                {
                    return;
                }

                txtFilePath.Text = dlg.FileName;
                dgRawData.DataSource = null;
                dgRawData.Refresh();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private async void UploadDataButtonClick(object sender, EventArgs e)
        {
            if (rawData.Count == 0)
            {
                MessageBox.Show("No data to upload");
                return;
            }

            Cursor = Cursors.WaitCursor;
            lblStatusMsg.Text = "Reading CSV file";
            Invalidate();

            try
            {
                await UploadSkillsetData();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                lblStatusMsg.Text = "Ready";
                Invalidate();
            }
        }

        private async void ReadCsvFileButtonClick(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                MessageBox.Show("Please select the Raw Skillset CSV file to read");
                txtFilePath.Focus();
                return;
            }

            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("The select raw file seems to be invalid");
                txtFilePath.Focus();
                return;
            }

            await ReadRawCsvFile();
        }

        private async Task ReadRawCsvFile()
        {
            Cursor = Cursors.WaitCursor;
            lblStatusMsg.Text = "Reading CSV file";
            Invalidate();

            StreamReader sr = null;
            rawData = new List<SkillsetData>();
            dgRawData.DataSource = null;

            try
            {
                sr = new StreamReader(File.OpenRead(txtFilePath.Text));
                while (!sr.EndOfStream)
                {
                    string sourceLine = await sr.ReadLineAsync();
                    string[] cols = null;
                    if (string.IsNullOrWhiteSpace(sourceLine))
                    {
                        continue;
                    }
                    cols = sourceLine.Split(',');
                    rawData.Add(new SkillsetData
                    {
                        EmpID = cols[2].Trim(),
                        LocationID = cols[1]?.Trim().ToLower() == "chennai" ? 542 : 543,
                        EmpName = cols[5].Trim(),
                        TotalExperience = cols[7].Trim(),
                        PrimarySkills = $"{cols[8].Trim()};{cols[9].Trim()};{cols[10].Trim()}",
                        SecordarySkills = $"{cols[11].Trim()};{cols[12].Trim()};{cols[13].Trim()}",
                        RLanguage = GetExperienceID(cols[15]),
                        SPSS = GetExperienceID(cols[16]),
                        SAS = GetExperienceID(cols[17]),
                        MachineLearning = GetExperienceID(cols[18]),
                        Kinesis = GetExperienceID(cols[19]),
                        Glue = GetExperienceID(cols[20]),
                        Athena = GetExperienceID(cols[21]),
                        BA = GetExperienceID(cols[22]),
                        JavaBigData = GetExperienceID(cols[23]),
                        DataPipeline = GetExperienceID(cols[24]),
                        Python = GetExperienceID(cols[25]),
                        Qubole = GetExperienceID(cols[26]),
                        CassandraBigData = GetExperienceID(cols[27]),
                        KafkaBigData = GetExperienceID(cols[28]),
                        HadoopBigData = GetExperienceID(cols[29]),
                        PigBigData = GetExperienceID(cols[30]),
                        CouchBase = GetExperienceID(cols[31]),
                        Neo4J = GetExperienceID(cols[32]),
                        MongoDB = GetExperienceID(cols[33]),
                        NoSQL = GetExperienceID(cols[34]),
                        GoogleBigQuery = GetExperienceID(cols[35]),
                        HBase = GetExperienceID(cols[36]),
                        Oozie = GetExperienceID(cols[37]),
                        Flume = GetExperienceID(cols[38]),
                        Hive = GetExperienceID(cols[39]),
                        Sqoop = GetExperienceID(cols[40]),
                        Scala = GetExperienceID(cols[41]),
                        MapReduce = GetExperienceID(cols[42]),
                        SparkDatabricks = GetExperienceID(cols[43]),
                        Storm = GetExperienceID(cols[44]),
                        AwsAdministration = GetExperienceID(cols[45]),
                        AWSEC2 = GetExperienceID(cols[46]),
                        AWSRedshift = GetExperienceID(cols[47]),
                        AWSElasticBeanStalk = GetExperienceID(cols[48]),
                        AWSLambda = GetExperienceID(cols[49]),
                        AWSVPC = GetExperienceID(cols[50]),
                        AWSIAM = GetExperienceID(cols[51]),
                        AWSRoute53 = GetExperienceID(cols[52]),
                        AWSSES = GetExperienceID(cols[53]),
                        Quicksight = GetExperienceID(cols[54]),
                        Elasticcache = GetExperienceID(cols[55]),
                        SQS = GetExperienceID(cols[56]),
                        Comprehend = GetExperienceID(cols[57]),
                        Aurora = GetExperienceID(cols[58]),
                        dynamoDB = GetExperienceID(cols[59]),
                        Stepfunctions = GetExperienceID(cols[60]),
                        DMS = GetExperienceID(cols[61]),
                        AWSSNS = GetExperienceID(cols[62]),
                        AWSCloudWatch = GetExperienceID(cols[63]),
                        AWSS3 = GetExperienceID(cols[64]),
                        AWSCloudFront = GetExperienceID(cols[65]),
                        AWSEFS = GetExperienceID(cols[66]),
                        AWSGlacier = GetExperienceID(cols[67]),
                        AWSRDS = GetExperienceID(cols[68]),
                        AWSCloudFormation = GetExperienceID(cols[69]),
                        AWSCloudTrail = GetExperienceID(cols[70]),
                        AWScloud = GetExperienceID(cols[71]),
                        AWSConfig = GetExperienceID(cols[72]),
                        AWSTrustedAdvisor = GetExperienceID(cols[73]),
                        AzureAD = GetExperienceID(cols[74]),
                        GoogleCloud = GetExperienceID(cols[75]),
                        Jenkins = GetExperienceID(cols[76]),
                        codepipeline = GetExperienceID(cols[77]),
                        codedeploy = GetExperienceID(cols[78]),
                        TeamCity = GetExperienceID(cols[79]),
                        Splunk = GetExperienceID(cols[80]),
                        RightScale = GetExperienceID(cols[81]),
                        Incapsula = GetExperienceID(cols[82]),
                        Chef = GetExperienceID(cols[83]),
                        Puppet = GetExperienceID(cols[84]),
                        BitBucket = GetExperienceID(cols[85]),
                        Ansible = GetExperienceID(cols[86]),
                        Terraform = GetExperienceID(cols[87]),
                        Joomla = GetExperienceID(cols[88]),
                        OpsWorks = GetExperienceID(cols[89]),
                        GitlabGithub = GetExperienceID(cols[90]),
                        SVN = GetExperienceID(cols[91]),
                        WordPressDrupalMagento = GetExperienceID(cols[92]),
                        TSQL = GetExperienceID(cols[93]),
                        PLSQL = GetExperienceID(cols[94]),
                        Informatica = GetExperienceID(cols[95]),
                        PerlScripting = GetExperienceID(cols[96]),
                        PentahoETL = GetExperienceID(cols[97]),
                        Snaplogic = GetExperienceID(cols[98]),
                        Mulesoft = GetExperienceID(cols[99]),
                        ODI = GetExperienceID(cols[100]),
                        TalendDI = GetExperienceID(cols[101]),
                        Attunuity = GetExperienceID(cols[102]),
                        DataStage = GetExperienceID(cols[103]),
                        SQLServer = GetExperienceID(cols[104]),
                        MySQL = GetExperienceID(cols[105]),
                        Postgres = GetExperienceID(cols[106]),
                        Redis = GetExperienceID(cols[107]),
                        Oracle = GetExperienceID(cols[108]),
                        Netezza = GetExperienceID(cols[109]),
                        Snowflake = GetExperienceID(cols[110]),
                        Teradata = GetExperienceID(cols[111]),
                        DB2 = GetExperienceID(cols[112]),
                        MainFrame = GetExperienceID(cols[113]),
                        SQLServerAdmin = GetExperienceID(cols[114]),
                        DBAdmin = GetExperienceID(cols[115]),
                        AdobeFlash = GetExperienceID(cols[116]),
                        AutoDesk3dsMax = GetExperienceID(cols[117]),
                        AutoDeskMaya = GetExperienceID(cols[118]),
                        AdobeAftersEffects = GetExperienceID(cols[119]),
                        Photoshop = GetExperienceID(cols[120]),
                        Magento = GetExperienceID(cols[121]),
                        WooCommerce = GetExperienceID(cols[122]),
                        CoreJava = GetExperienceID(cols[123]),
                        Json = GetExperienceID(cols[124]),
                        SpringFramework = GetExperienceID(cols[125]),
                        Hibernate = GetExperienceID(cols[126]),
                        JUnit = GetExperienceID(cols[127]),
                        Maven = GetExperienceID(cols[128]),
                        Ant = GetExperienceID(cols[129]),
                        Gradle = GetExperienceID(cols[130]),
                        Multithreading = GetExperienceID(cols[131]),
                        JMS = GetExperienceID(cols[132]),
                        MQ = GetExperienceID(cols[133]),
                        Webservices = GetExperienceID(cols[134]),
                        Intellijeclipse = GetExperienceID(cols[135]),
                        J2ee = GetExperienceID(cols[136]),
                        Ibatis = GetExperienceID(cols[137]),
                        EJB = GetExperienceID(cols[138]),
                        Struts = GetExperienceID(cols[139]),
                        JSP = GetExperienceID(cols[140]),
                        _Net = GetExperienceID(cols[141]),
                        BizTalk = GetExperienceID(cols[142]),
                        ASP_NET = GetExperienceID(cols[143]),
                        VB = GetExperienceID(cols[144]),
                        MSBICubes = GetExperienceID(cols[145]),
                        SharePoint = GetExperienceID(cols[146]),
                        SSAS = GetExperienceID(cols[147]),
                        SSRS = GetExperienceID(cols[148]),
                        SSIS = GetExperienceID(cols[149]),
                        VisualStudio = GetExperienceID(cols[150]),
                        CSharp = GetExperienceID(cols[151]),
                        PaymentGatewayIntegration = GetExperienceID(cols[152]),
                        SSOintegration = GetExperienceID(cols[153]),
                        Android = GetExperienceID(cols[154]),
                        CloudDevopsNOCServices = GetExperienceID(cols[155]),
                        ChefNOCServices = GetExperienceID(cols[156]),
                        LinuxNOCServices = GetExperienceID(cols[157]),
                        Astyanax = GetExperienceID(cols[158]),
                        Nagios = GetExperienceID(cols[159]),
                        Zabbix = GetExperienceID(cols[160]),
                        Haproxy = GetExperienceID(cols[161]),
                        Asterisk = GetExperienceID(cols[162]),
                        PRTG = GetExperienceID(cols[163]),
                        QA = GetExperienceID(cols[164]),
                        Jmeter = GetExperienceID(cols[165]),
                        Selenium = GetExperienceID(cols[166]),
                        UFT = GetExperienceID(cols[167]),
                        VisualStudioCodedUI = GetExperienceID(cols[168]),
                        VSTestManager = GetExperienceID(cols[169]),
                        QualityCenter = GetExperienceID(cols[170]),
                        MSDynamicCRMTesting = GetExperienceID(cols[171]),
                        SoapUI = GetExperienceID(cols[172]),
                        IBMRationalProducts = GetExperienceID(cols[173]),
                        SAPBW = GetExperienceID(cols[174]),
                        SAPBO = GetExperienceID(cols[175]),
                        SAPHana = GetExperienceID(cols[176]),
                        SAPBODS = GetExperienceID(cols[177]),
                        SAPLumira = GetExperienceID(cols[178]),
                        SAPDesignStudio = GetExperienceID(cols[179]),
                        SAPABAP = GetExperienceID(cols[180]),
                        SAPwebDynpro = GetExperienceID(cols[181]),
                        SAPLeonardoIOT = GetExperienceID(cols[182]),
                        ElasticsearchCloudsearchSolr = GetExperienceID(cols[183]),
                        Kibana = GetExperienceID(cols[184]),
                        Level1Support = GetExperienceID(cols[185]),
                        BashScripting = GetExperienceID(cols[186]),
                        Microstrategy = GetExperienceID(cols[187]),
                        Jasperreport = GetExperienceID(cols[188]),
                        Tableau = GetExperienceID(cols[189]),
                        IBMCognosTM1BI = GetExperienceID(cols[190]),
                        ExcelMacrosVBA = GetExperienceID(cols[191]),
                        ExcelDashboards = GetExperienceID(cols[192]),
                        Looker = GetExperienceID(cols[193]),
                        QlikviewQliksense = GetExperienceID(cols[194]),
                        PowerBI = GetExperienceID(cols[195]),
                        ApacheNginx = GetExperienceID(cols[196]),
                        TomcatServer = GetExperienceID(cols[197]),
                        JbossServer = GetExperienceID(cols[198]),
                        WeblogicServers = GetExperienceID(cols[199]),
                        HTML = GetExperienceID(cols[200]),
                        AngularJS = GetExperienceID(cols[201]),
                        Angular = GetExperienceID(cols[202]),
                        Javascript = GetExperienceID(cols[203]),
                        GWT = GetExperienceID(cols[204]),
                        nodejs = GetExperienceID(cols[205]),
                        ReactJS = GetExperienceID(cols[206]),
                        PHP = GetExperienceID(cols[207]),
                        XML = GetExperienceID(cols[208]),
                        AJAX = GetExperienceID(cols[209]),
                        CSS3 = GetExperienceID(cols[210]),
                        EJS = GetExperienceID(cols[211]),
                        RafaelJS = GetExperienceID(cols[212]),
                        CodeIgniter = GetExperienceID(cols[213]),
                        Rubyonrails = GetExperienceID(cols[214]),
                        Grunt = GetExperienceID(cols[215]),
                        AtmosphereFramework = GetExperienceID(cols[216]),
                        PlayFramework = GetExperienceID(cols[217]),
                        Jquery = GetExperienceID(cols[218]),
                        Bootstrap = GetExperienceID(cols[219]),
                        Postman = GetExperienceID(cols[220]),
                        Akka = GetExperienceID(cols[221]),
                        LoadRunner = GetExperienceID(cols[222]),
                        Naturallanguage = GetExperienceID(cols[223]),
                        Adabas = GetExperienceID(cols[224]),
                        JCL = GetExperienceID(cols[225]),
                        COBOL = GetExperienceID(cols[226]),
                        BigQuery = GetExperienceID(cols[227]),
                        CloudDataflow = GetExperienceID(cols[228]),
                        CloudPubSub = GetExperienceID(cols[229]),
                        CloudBigtable = GetExperienceID(cols[230]),
                        CloudStorage = GetExperienceID(cols[231]),
                        CloudDataproc = GetExperienceID(cols[232]),
                        CloudDataFusion = GetExperienceID(cols[233]),
                        CloudComposer = GetExperienceID(cols[234]),
                        GoogleDataStudio = GetExperienceID(cols[235]),
                        CloudDataTransfer = GetExperienceID(cols[236]),
                        CloudDatalab = GetExperienceID(cols[237]),
                        DataCatalog = GetExperienceID(cols[238]),
                        GoogleSheets = GetExperienceID(cols[239]),
                        CloudDataprep = GetExperienceID(cols[240]),
                        CloudSQL = GetExperienceID(cols[241]),
                        PersistentDisk = GetExperienceID(cols[242]),
                        CloudSpanner = GetExperienceID(cols[243]),
                        CloudMemorystore = GetExperienceID(cols[244]),
                        CloudFirestore = GetExperienceID(cols[245]),
                        FirebaseRealtimeDatabase = GetExperienceID(cols[246]),
                        GoogleComputeEngine = GetExperienceID(cols[247]),
                        GoogleAppEngine = GetExperienceID(cols[248]),
                        GoogleKubernetesEngine = GetExperienceID(cols[249]),
                        CloudFunctions = GetExperienceID(cols[250]),
                        GoogleCloudContainerRegistry = GetExperienceID(cols[251]),
                        AIHub = GetExperienceID(cols[252]),
                        NaturalLanguage = GetExperienceID(cols[253]),
                        AutoMLTranslation = GetExperienceID(cols[254]),
                        CloudSpeech_to_TextAPI = GetExperienceID(cols[255]),
                        CloudText_to_SpeechAPI = GetExperienceID(cols[256]),
                        DialogflowEnterpriseEdition = GetExperienceID(cols[257]),
                        CloudInferenceAPI = GetExperienceID(cols[258]),
                        RecommendationsAI = GetExperienceID(cols[259]),
                        BigQueryML = GetExperienceID(cols[260]),
                        TensorFlow = GetExperienceID(cols[261]),
                        VisionAI = GetExperienceID(cols[262]),
                        VideoAI = GetExperienceID(cols[263]),
                        AutoMLTables = GetExperienceID(cols[264]),
                        CloudAutoML = GetExperienceID(cols[265]),
                        AIPlatformNotebooks = GetExperienceID(cols[266]),
                        AIPlatformDeepLearningVMImage = GetExperienceID(cols[267]),
                        CloudTPU = GetExperienceID(cols[268]),
                        Kubeflow = GetExperienceID(cols[269]),
                    });
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
                Cursor = Cursors.Default;
                lblStatusMsg.Text = "Ready";
                Invalidate();
            }
            dgRawData.DataSource = rawData;
        }

        private async Task UploadSkillsetData()
        {
            StringBuilder errors = new StringBuilder();
            errors.Append($"Failed records{Environment.NewLine}");
            try
            {
                repository = new EmployeeTechRepository();

                for (int i = 1; i < rawData.Count; i++)
                {
                    try
                    {
                        lblStatusMsg.Text = $"Processing Employee: {rawData[i].EmpID}";
                        EmpAssetDetailDto empAssetDetail = new EmpAssetDetailDto
                        {
                            EmployeeID = rawData[i].EmpID.Trim(),
                            LocationID = rawData[i].LocationID,
                            OverallExperience = rawData[i].TotalExperience,
                            PrimarySkills = rawData[i].PrimarySkills,
                            SecondarySkills = rawData[i].SecordarySkills,
                        };

                        int assetEntryID = repository.UpdateEmployeeDetails(empAssetDetail);
                        empAssetDetail.EmpAssetDetailID = assetEntryID;
                        await ProcessAddEmpSkills(rawData[i], empAssetDetail);
                    }
                    catch (Exception exp1)
                    {
                        errors.Append($"{rawData[i].EmpID} - {exp1.Message}-{exp1.InnerException?.Message}{Environment.NewLine}");
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show(exp.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private string GetExperienceID(string exp)
        {
            int expID = 0;
            switch (exp)
            {
                case "1":
                    expID = 544;
                    break;
                case "2":
                    expID = 545;
                    break;
                case "3":
                    expID = 546;
                    break;
                case "4":
                    expID = 547;
                    break;
                case "5":
                    expID = 548;
                    break;
            }

            return expID == 0 ? "" : expID.ToString();
        }

        private async Task ProcessAddEmpSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            try
            {
                #region BA Skills

                if (!string.IsNullOrWhiteSpace(skillsetData.BA))
                {
                    await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BA), (int)SkillCategory.BA, (int)BaSkills.BA);
                }

                #endregion

                //lblStatusMsg.Text = "Adding Analytic skills";
                try
                {
                    await ProcessAnalyticSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Big Data skills";
                try
                {
                    await ProcessBigDataSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Cloud AWS skills";
                try
                {
                    await ProcessCloudAwsSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Cloud Azure skills";
                try
                {
                    await ProcessCloudAzureSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Cloud GCS skills";
                try
                {
                    await ProcessCloudGcsSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Dev Ops skills";
                try
                {
                    await ProcessDevOpsSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding CMS skills";
                try
                {
                    await ProcessCmsSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Config. Management skills";
                try
                {
                    await ProcessConfigMagmtSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Content Management System skills";
                try
                {
                    await ProcessContentMgmtSysSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Data Integration skills";
                try
                {
                    await ProcessDataIntegrationSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Database skills";
                try
                {
                    await ProcessDatabaseSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding DBA skills";
                try
                {
                    await ProcessDbaSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Design skills";
                try
                {
                    await ProcessDesignSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding E-Commerce skills";
                try
                {
                    await ProcessECommerceSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Java skills";
                try
                {
                    await ProcessJavaSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Microsoft skills";
                try
                {
                    await ProcessMicrosoftSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Misc skills";
                try
                {
                    await ProcessMiscSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Mobile skills";
                try
                {
                    await ProcessMobileSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding NOC skills";
                try
                {
                    await ProcessNocSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding QA skills";
                try
                {
                    await ProcessQASkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding SAP BI skills";
                try
                {
                    await ProcessSapBISkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Search and Analytics skills";
                try
                {
                    await ProcessSearchAnalyticSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Support skills";
                try
                {
                    await ProcessSupportSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Unix skills";
                try
                {
                    await ProcessUnixSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Visualization skills";
                try
                {
                    await ProcessVisualizationSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Web Server skills";
                try
                {
                    await ProcessWebServerSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Web Tech skills";
                try
                {
                    await ProcessWebTechSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Tool skills";
                try
                {
                    await ProcessToolSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Mainframe skills";
                try
                {
                    await ProcessMainframeSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }
                //lblStatusMsg.Text = "Adding Google skills";
                try
                {
                    await ProcessGoogleSkills(skillsetData, empAssetDetail);
                }
                catch (Exception) { }

            }
            catch (Exception)
            {

            }
        }

        private async Task AddEmpSkill(int empEntryID, string empID, int rating, int category, int skill)
        {
            try
            {
                EmployeeSkillDto skillDto = new EmployeeSkillDto
                {
                    EmployeeEntryID = empEntryID,
                    EmployeeID = empID,
                    RatingID = rating,
                    SkillCategoryID = category,
                    TechSkillID = skill,
                };
                await repository.AddEmpSkillAsync(skillDto);
            }
            catch (Exception) { }
        }

        private async Task ProcessAnalyticSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.RLanguage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.RLanguage), (int)SkillCategory.Analytics, (int)AnalyticsSkills.RLanguage);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SPSS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SPSS), (int)SkillCategory.Analytics, (int)AnalyticsSkills.SPSS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAS), (int)SkillCategory.Analytics, (int)AnalyticsSkills.SAS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MachineLearning))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MachineLearning), (int)SkillCategory.Analytics, (int)AnalyticsSkills.MachineLearning);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Kinesis))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Kinesis), (int)SkillCategory.Analytics, (int)AnalyticsSkills.Kinesis);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Glue))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Glue), (int)SkillCategory.Analytics, (int)AnalyticsSkills.Glue);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Athena))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Athena), (int)SkillCategory.Analytics, (int)AnalyticsSkills.Athena);
            }
        }

        private async Task ProcessBigDataSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.JavaBigData))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JavaBigData), (int)SkillCategory.BigData, (int)BigDataSkills.JavaBigData);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.DataPipeline))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DataPipeline), (int)SkillCategory.BigData, (int)BigDataSkills.DataPipeline);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Python))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Python), (int)SkillCategory.BigData, (int)BigDataSkills.Python);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Qubole))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Qubole), (int)SkillCategory.BigData, (int)BigDataSkills.Qubole);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CassandraBigData))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CassandraBigData), (int)SkillCategory.BigData, (int)BigDataSkills.CassandraBigData);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.KafkaBigData))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.KafkaBigData), (int)SkillCategory.BigData, (int)BigDataSkills.KafkaBigData);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.HadoopBigData))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.HadoopBigData), (int)SkillCategory.BigData, (int)BigDataSkills.HadoopBigData);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PigBigData))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PigBigData), (int)SkillCategory.BigData, (int)BigDataSkills.PigBigData);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CouchBase))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CouchBase), (int)SkillCategory.BigData, (int)BigDataSkills.CouchBase);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Neo4J))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Neo4J), (int)SkillCategory.BigData, (int)BigDataSkills.Neo4J);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MongoDB))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MongoDB), (int)SkillCategory.BigData, (int)BigDataSkills.MongoDB);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.NoSQL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.NoSQL), (int)SkillCategory.BigData, (int)BigDataSkills.NoSQL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleBigQuery))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleBigQuery), (int)SkillCategory.BigData, (int)BigDataSkills.GoogleBigQuery);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.HBase))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.HBase), (int)SkillCategory.BigData, (int)BigDataSkills.Hbase);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Oozie))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Oozie), (int)SkillCategory.BigData, (int)BigDataSkills.Oozie);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Flume))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Flume), (int)SkillCategory.BigData, (int)BigDataSkills.Flume);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Hive))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Hive), (int)SkillCategory.BigData, (int)BigDataSkills.Hive);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Sqoop))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Sqoop), (int)SkillCategory.BigData, (int)BigDataSkills.SQoop);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Scala))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Scala), (int)SkillCategory.BigData, (int)BigDataSkills.Scala);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MapReduce))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MapReduce), (int)SkillCategory.BigData, (int)BigDataSkills.MapReduce);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SparkDatabricks))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SparkDatabricks), (int)SkillCategory.BigData, (int)BigDataSkills.SparDatabricks);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Storm))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Storm), (int)SkillCategory.BigData, (int)BigDataSkills.Storm);
            }
        }

        private async Task ProcessCloudAwsSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.AwsAdministration))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AwsAdministration), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSAdministration);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSEC2))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSEC2), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSEC2);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSRedshift))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSRedshift), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSRedshift);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSElasticBeanStalk))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSElasticBeanStalk), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSElasticBeanStalk);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSLambda))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSLambda), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSLambda);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSVPC))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSVPC), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSVPC);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSIAM))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSIAM), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSIAM);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSRoute53))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSRoute53), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSRoute53);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSSES))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSSES), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSES);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Quicksight))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Quicksight), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.QuickSight);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Elasticcache))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Elasticcache), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.ElasticCache);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SQS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SQS), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.SQS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Comprehend))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Comprehend), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.Comprehend);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Aurora))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Aurora), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.Aurora);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.dynamoDB))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.dynamoDB), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.DynamoDB);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Stepfunctions))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Stepfunctions), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.StepFunctions);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Stepfunctions))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Stepfunctions), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.StepFunctions);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.DMS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DMS), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.DMS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSSNS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSSNS), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSSNS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSCloudWatch))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSCloudWatch), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSCloudWatch);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSS3))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSS3), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSS3);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSCloudFront))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSCloudFront), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSCloudFront);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSEFS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSEFS), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSEFS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSGlacier))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSGlacier), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSGlacier);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSRDS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSRDS), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSRDS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSCloudFormation))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSCloudFormation), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSCloudFormation);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSCloudTrail))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSCloudTrail), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSCloudTrail);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWScloud))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWScloud), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSCloud);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSConfig))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSConfig), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSConfig);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AWSTrustedAdvisor))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AWSTrustedAdvisor), (int)SkillCategory.CloudAWS, (int)CloudAwsSkills.AWSTrustedAdvisor);
            }
        }

        private async Task ProcessCloudAzureSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.AzureAD))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AzureAD), (int)SkillCategory.CloudAzure, (int)CloudAzureSkills.AzureAD);
            }
        }

        private async Task ProcessCloudGcsSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleCloud))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleCloud), (int)SkillCategory.CloudGCS, (int)CloudGcsSkills.GoogleCloud);
            }
        }

        private async Task ProcessDevOpsSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Jenkins))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Jenkins), (int)SkillCategory.DevOps, (int)DevOpsSkills.Jenkins);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.codepipeline))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.codepipeline), (int)SkillCategory.DevOps, (int)DevOpsSkills.CodePipeline);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.codedeploy))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.codedeploy), (int)SkillCategory.DevOps, (int)DevOpsSkills.CodeDeploy);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.TeamCity))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.TeamCity), (int)SkillCategory.DevOps, (int)DevOpsSkills.TeamCity);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Splunk))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Splunk), (int)SkillCategory.DevOps, (int)DevOpsSkills.Splunk);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.RightScale))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.RightScale), (int)SkillCategory.DevOps, (int)DevOpsSkills.RightScale);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Incapsula))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Incapsula), (int)SkillCategory.DevOps, (int)DevOpsSkills.Incapsula);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Chef))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Chef), (int)SkillCategory.DevOps, (int)DevOpsSkills.Chef);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Puppet))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Puppet), (int)SkillCategory.DevOps, (int)DevOpsSkills.Puppet);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.BitBucket))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BitBucket), (int)SkillCategory.DevOps, (int)DevOpsSkills.BitBucket);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Ansible))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Ansible), (int)SkillCategory.DevOps, (int)DevOpsSkills.Ansible);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Terraform))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Terraform), (int)SkillCategory.DevOps, (int)DevOpsSkills.Terraform);
            }
        }

        private async Task ProcessCmsSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Joomla))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Joomla), (int)SkillCategory.CMS, (int)CmsSkills.Joomla);
            }
        }

        private async Task ProcessConfigMagmtSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.GitlabGithub))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GitlabGithub), (int)SkillCategory.ConfigManagement, (int)ConfigManagementSkills.GitlabGithub);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.OpsWorks))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.OpsWorks), (int)SkillCategory.ConfigManagement, (int)ConfigManagementSkills.OpsWorks);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SVN))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SVN), (int)SkillCategory.ConfigManagement, (int)ConfigManagementSkills.SVN);
            }
        }

        private async Task ProcessContentMgmtSysSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.WordPressDrupalMagento))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.WordPressDrupalMagento), (int)SkillCategory.ContentManagementSystem, (int)ContentManagementSysSkills.WordPressDrupalMagento);
            }
        }

        private async Task ProcessDataIntegrationSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Attunuity))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Attunuity), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.Attunuity);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.DataStage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DataStage), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.DataStage);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Informatica))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Informatica), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.Informatica);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Mulesoft))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Mulesoft), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.Mulesoft);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.ODI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ODI), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.ODI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PentahoETL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PentahoETL), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.PentahoETL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PerlScripting))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PerlScripting), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.PerlScripting);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PLSQL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PLSQL), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.PLSQL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Snaplogic))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Snaplogic), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.Snaplogic);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.TalendDI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.TalendDI), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.TalendDI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.TSQL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.TSQL), (int)SkillCategory.DataIntegration, (int)DataIntegrationSkills.TSQL);
            }
        }

        private async Task ProcessDatabaseSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.DB2))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DB2), (int)SkillCategory.Database, (int)DatabaseSkills.DB2);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MainFrame))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MainFrame), (int)SkillCategory.Database, (int)DatabaseSkills.MainFrame);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MySQL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MySQL), (int)SkillCategory.Database, (int)DatabaseSkills.MySQL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Netezza))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Netezza), (int)SkillCategory.Database, (int)DatabaseSkills.Netezza);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Oracle))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Oracle), (int)SkillCategory.Database, (int)DatabaseSkills.Oracle);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Postgres))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Postgres), (int)SkillCategory.Database, (int)DatabaseSkills.Postgres);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Redis))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Redis), (int)SkillCategory.Database, (int)DatabaseSkills.Redis);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Snowflake))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Snowflake), (int)SkillCategory.Database, (int)DatabaseSkills.Snowflake);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SQLServer))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SQLServer), (int)SkillCategory.Database, (int)DatabaseSkills.SQLServer);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Teradata))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Teradata), (int)SkillCategory.Database, (int)DatabaseSkills.Teradata);
            }
        }

        private async Task ProcessDbaSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.DBAdmin))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DBAdmin), (int)SkillCategory.DBA, (int)DbaSkills.DbAdmin);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SQLServerAdmin))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SQLServerAdmin), (int)SkillCategory.DBA, (int)DbaSkills.SqlServerAdmin);
            }
        }

        private async Task ProcessDesignSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.AdobeAftersEffects))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AdobeAftersEffects), (int)SkillCategory.Design, (int)DesignSkills.AdobeAftersEffects);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AdobeFlash))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AdobeFlash), (int)SkillCategory.Design, (int)DesignSkills.AdobeFlash);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AutoDesk3dsMax))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AutoDesk3dsMax), (int)SkillCategory.Design, (int)DesignSkills.AutoDesk3dsMax);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AutoDeskMaya))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AutoDeskMaya), (int)SkillCategory.Design, (int)DesignSkills.AutoDeskMaya);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Photoshop))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Photoshop), (int)SkillCategory.Design, (int)DesignSkills.Photoshop);
            }
        }

        private async Task ProcessECommerceSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Magento))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Magento), (int)SkillCategory.ECommerce, (int)ECommerceSkills.Magento);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.WooCommerce))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.WooCommerce), (int)SkillCategory.ECommerce, (int)ECommerceSkills.WooCommerce);
            }
        }

        private async Task ProcessJavaSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Ant))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Ant), (int)SkillCategory.JavaStack, (int)JavaSkills.Ant);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CoreJava))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CoreJava), (int)SkillCategory.JavaStack, (int)JavaSkills.CoreJava);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.EJB))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.EJB), (int)SkillCategory.JavaStack, (int)JavaSkills.EJB);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Gradle))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Gradle), (int)SkillCategory.JavaStack, (int)JavaSkills.Gradle);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Hibernate))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Hibernate), (int)SkillCategory.JavaStack, (int)JavaSkills.Hibernate);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Ibatis))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Ibatis), (int)SkillCategory.JavaStack, (int)JavaSkills.Ibatis);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Intellijeclipse))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Intellijeclipse), (int)SkillCategory.JavaStack, (int)JavaSkills.IntellijEclipse);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.J2ee))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.J2ee), (int)SkillCategory.JavaStack, (int)JavaSkills.J2ee);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.JMS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JMS), (int)SkillCategory.JavaStack, (int)JavaSkills.JMS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Json))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Json), (int)SkillCategory.JavaStack, (int)JavaSkills.Json);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.JSP))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JSP), (int)SkillCategory.JavaStack, (int)JavaSkills.JSP);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.JUnit))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JUnit), (int)SkillCategory.JavaStack, (int)JavaSkills.JUnit);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Maven))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Maven), (int)SkillCategory.JavaStack, (int)JavaSkills.Maven);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MQ))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MQ), (int)SkillCategory.JavaStack, (int)JavaSkills.MQ);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Multithreading))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Multithreading), (int)SkillCategory.JavaStack, (int)JavaSkills.MultiThreading);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SpringFramework))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SpringFramework), (int)SkillCategory.JavaStack, (int)JavaSkills.SpringFramework);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Struts))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Struts), (int)SkillCategory.JavaStack, (int)JavaSkills.Struts);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Webservices))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Webservices), (int)SkillCategory.JavaStack, (int)JavaSkills.WebServices);
            }
        }

        private async Task ProcessMicrosoftSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.ASP_NET))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ASP_NET), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.ASP_NET);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.BizTalk))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BizTalk), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.BizTalk);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CSharp))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CSharp), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.CSharp);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData._Net))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData._Net), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.DotNet);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MSBICubes))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MSBICubes), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.MSBICubes);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SharePoint))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SharePoint), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.SharePoint);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SSAS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SSAS), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.SSAS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SSIS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SSIS), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.SSIS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SSRS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SSRS), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.SSRS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VB))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VB), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.VB);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VisualStudio))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VisualStudio), (int)SkillCategory.MicrosoftStack, (int)MicrosoftSkills.VisualStudio);
            }
        }

        private async Task ProcessMiscSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.PaymentGatewayIntegration))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PaymentGatewayIntegration), (int)SkillCategory.Misc, (int)MiscSkills.PaymentGatewayIntegration);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SSOintegration))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SSOintegration), (int)SkillCategory.Misc, (int)MiscSkills.SSOIntegration);
            }
        }

        private async Task ProcessMobileSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Android))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Android), (int)SkillCategory.Mobile, (int)MobileSkills.Android);
            }
        }

        private async Task ProcessNocSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Asterisk))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Asterisk), (int)SkillCategory.NOC, (int)NocSkills.Asterisk);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Astyanax))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Astyanax), (int)SkillCategory.NOC, (int)NocSkills.Astyanax);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.ChefNOCServices))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ChefNOCServices), (int)SkillCategory.NOC, (int)NocSkills.ChefNOCServices);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDevopsNOCServices))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDevopsNOCServices), (int)SkillCategory.NOC, (int)NocSkills.CloudDevopsNOCServices);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Haproxy))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Haproxy), (int)SkillCategory.NOC, (int)NocSkills.Haproxy);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.LinuxNOCServices))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.LinuxNOCServices), (int)SkillCategory.NOC, (int)NocSkills.LinuxNOCServices);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Nagios))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Nagios), (int)SkillCategory.NOC, (int)NocSkills.Nagios);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PRTG))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PRTG), (int)SkillCategory.NOC, (int)NocSkills.PRTG);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Zabbix))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Zabbix), (int)SkillCategory.NOC, (int)NocSkills.Zabbix);
            }
        }

        private async Task ProcessQASkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.IBMRationalProducts))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.IBMRationalProducts), (int)SkillCategory.QA, (int)QaSkills.IBMRationalProducts);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Jmeter))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Jmeter), (int)SkillCategory.QA, (int)QaSkills.Jmeter);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.MSDynamicCRMTesting))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.MSDynamicCRMTesting), (int)SkillCategory.QA, (int)QaSkills.MSDynamicCRMTesting);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.QA))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.QA), (int)SkillCategory.QA, (int)QaSkills.QA);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.QualityCenter))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.QualityCenter), (int)SkillCategory.QA, (int)QaSkills.QualityCenter);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Selenium))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Selenium), (int)SkillCategory.QA, (int)QaSkills.Selenium);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SoapUI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SoapUI), (int)SkillCategory.QA, (int)QaSkills.SoapUI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.UFT))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.UFT), (int)SkillCategory.QA, (int)QaSkills.UFT);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VisualStudioCodedUI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VisualStudioCodedUI), (int)SkillCategory.QA, (int)QaSkills.VisualStudioCodedUI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VSTestManager))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VSTestManager), (int)SkillCategory.QA, (int)QaSkills.VSTestManager);
            }
        }

        private async Task ProcessSapBISkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPABAP))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPABAP), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPABAP);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPBO))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPBO), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPBO);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPBODS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPBODS), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPBODS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPBW))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPBW), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPBW);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPDesignStudio))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPDesignStudio), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPDesignStudio);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPHana))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPHana), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPHana);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPLeonardoIOT))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPLeonardoIOT), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPLeonardoIOT);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPLumira))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPLumira), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPLumira);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.SAPwebDynpro))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.SAPwebDynpro), (int)SkillCategory.SapBI, (int)SapBiSkills.SAPWebDynpro);
            }
        }

        private async Task ProcessSearchAnalyticSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.ElasticsearchCloudsearchSolr))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ElasticsearchCloudsearchSolr), (int)SkillCategory.SearchAndAnalytics, (int)SearchAndAnalyticSkills.ElasticsearchCloudsearchSolr);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Kibana))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Kibana), (int)SkillCategory.SearchAndAnalytics, (int)SearchAndAnalyticSkills.Kibana);
            }
        }

        private async Task ProcessSupportSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Level1Support))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Level1Support), (int)SkillCategory.Support, (int)SupportSkills.LevelOneSupport);
            }
        }

        private async Task ProcessUnixSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.BashScripting))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BashScripting), (int)SkillCategory.Unix, (int)UnixSkills.BashScripting);
            }
        }

        private async Task ProcessVisualizationSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.ExcelDashboards))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ExcelDashboards), (int)SkillCategory.Visualization, (int)VisualizationSkills.ExcelDashboards);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.ExcelMacrosVBA))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ExcelMacrosVBA), (int)SkillCategory.Visualization, (int)VisualizationSkills.ExcelMacrosVBA);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.IBMCognosTM1BI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.IBMCognosTM1BI), (int)SkillCategory.Visualization, (int)VisualizationSkills.IBMCognosTM1BI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Jasperreport))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Jasperreport), (int)SkillCategory.Visualization, (int)VisualizationSkills.JasperReport);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Looker))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Looker), (int)SkillCategory.Visualization, (int)VisualizationSkills.Looker);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Microstrategy))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Microstrategy), (int)SkillCategory.Visualization, (int)VisualizationSkills.Microstrategy);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PowerBI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PowerBI), (int)SkillCategory.Visualization, (int)VisualizationSkills.PowerBI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.QlikviewQliksense))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.QlikviewQliksense), (int)SkillCategory.Visualization, (int)VisualizationSkills.QlikviewQliksense);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Tableau))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Tableau), (int)SkillCategory.Visualization, (int)VisualizationSkills.Tableau);
            }
        }

        private async Task ProcessWebServerSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.ApacheNginx))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ApacheNginx), (int)SkillCategory.WebServer, (int)WebServerSkills.ApacheNginx);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.JbossServer))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JbossServer), (int)SkillCategory.WebServer, (int)WebServerSkills.JbossServer);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.TomcatServer))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.TomcatServer), (int)SkillCategory.WebServer, (int)WebServerSkills.TomcatServer);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.WeblogicServers))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.WeblogicServers), (int)SkillCategory.WebServer, (int)WebServerSkills.WeblogicServers);
            }
        }

        private async Task ProcessWebTechSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.AJAX))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AJAX), (int)SkillCategory.WebTech, (int)WebTechnologySkills.AJAX);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Akka))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Akka), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Akka);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Angular))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Angular), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Angular);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AngularJS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AngularJS), (int)SkillCategory.WebTech, (int)WebTechnologySkills.AngularJS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AtmosphereFramework))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AtmosphereFramework), (int)SkillCategory.WebTech, (int)WebTechnologySkills.AtmosphereFramework);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Bootstrap))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Bootstrap), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Bootstrap);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CodeIgniter))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CodeIgniter), (int)SkillCategory.WebTech, (int)WebTechnologySkills.CodeIgniter);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CSS3))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CSS3), (int)SkillCategory.WebTech, (int)WebTechnologySkills.CSS3);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.EJS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.EJS), (int)SkillCategory.WebTech, (int)WebTechnologySkills.EJS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Grunt))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Grunt), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Grunt);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GWT))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GWT), (int)SkillCategory.WebTech, (int)WebTechnologySkills.GWT);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.HTML))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.HTML), (int)SkillCategory.WebTech, (int)WebTechnologySkills.HTML);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Javascript))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Javascript), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Javascript);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Jquery))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Jquery), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Jquery);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.nodejs))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.nodejs), (int)SkillCategory.WebTech, (int)WebTechnologySkills.NodeJS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PHP))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PHP), (int)SkillCategory.WebTech, (int)WebTechnologySkills.PHP);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PlayFramework))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PlayFramework), (int)SkillCategory.WebTech, (int)WebTechnologySkills.PlayFramework);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Postman))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Postman), (int)SkillCategory.WebTech, (int)WebTechnologySkills.Postman);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.RafaelJS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.RafaelJS), (int)SkillCategory.WebTech, (int)WebTechnologySkills.RafaelJS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.ReactJS))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.ReactJS), (int)SkillCategory.WebTech, (int)WebTechnologySkills.ReactJS);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Rubyonrails))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Rubyonrails), (int)SkillCategory.WebTech, (int)WebTechnologySkills.RubyOnRails);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.XML))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.XML), (int)SkillCategory.WebTech, (int)WebTechnologySkills.XML);
            }
        }

        private async Task ProcessToolSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.LoadRunner))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.LoadRunner), (int)SkillCategory.Tools, (int)ToolsSkills.LoadRunner);
            }
        }

        private async Task ProcessMainframeSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.Adabas))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Adabas), (int)SkillCategory.Mainframe, (int)MainframeSkills.Adabas);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.COBOL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.COBOL), (int)SkillCategory.Mainframe, (int)MainframeSkills.COBOL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.JCL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.JCL), (int)SkillCategory.Mainframe, (int)MainframeSkills.JCL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.NaturalLanguage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.NaturalLanguage), (int)SkillCategory.Mainframe, (int)MainframeSkills.NaturalLanguage);
            }
        }

        private async Task ProcessGoogleSkills(SkillsetData skillsetData, EmpAssetDetailDto empAssetDetail)
        {
            if (!string.IsNullOrWhiteSpace(skillsetData.AIHub))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AIHub), (int)SkillCategory.Google, (int)GoogleSkills.AIHub);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AIPlatformDeepLearningVMImage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AIPlatformDeepLearningVMImage), (int)SkillCategory.Google, (int)GoogleSkills.AIPlatformDeepLearningVMImage);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AIPlatformNotebooks))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AIPlatformNotebooks), (int)SkillCategory.Google, (int)GoogleSkills.AIPlatformNotebooks);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AutoMLTables))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AutoMLTables), (int)SkillCategory.Google, (int)GoogleSkills.AutoMLTables);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.AutoMLTranslation))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.AutoMLTranslation), (int)SkillCategory.Google, (int)GoogleSkills.AutoMLTranslation);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.BigQuery))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BigQuery), (int)SkillCategory.Google, (int)GoogleSkills.BigQuery);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.BigQueryML))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.BigQueryML), (int)SkillCategory.Google, (int)GoogleSkills.BigQueryML);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudAutoML))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudAutoML), (int)SkillCategory.Google, (int)GoogleSkills.CloudAutoML);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudBigtable))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudBigtable), (int)SkillCategory.Google, (int)GoogleSkills.CloudBigtable);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudComposer))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Adabas), (int)SkillCategory.Google, (int)GoogleSkills.CloudComposer);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDataflow))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDataflow), (int)SkillCategory.Google, (int)GoogleSkills.CloudDataflow);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDataFusion))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDataFusion), (int)SkillCategory.Google, (int)GoogleSkills.CloudDataFusion);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDatalab))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDatalab), (int)SkillCategory.Google, (int)GoogleSkills.CloudDatalab);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDataprep))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDataprep), (int)SkillCategory.Google, (int)GoogleSkills.CloudDataprep);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDataproc))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDataproc), (int)SkillCategory.Google, (int)GoogleSkills.CloudDataproc);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudDataTransfer))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudDataTransfer), (int)SkillCategory.Google, (int)GoogleSkills.CloudDataTransfer);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudFirestore))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudFirestore), (int)SkillCategory.Google, (int)GoogleSkills.CloudFirestore);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudFunctions))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudFunctions), (int)SkillCategory.Google, (int)GoogleSkills.CloudFunctions);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudInferenceAPI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudInferenceAPI), (int)SkillCategory.Google, (int)GoogleSkills.CloudInferenceAPI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudMemorystore))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudMemorystore), (int)SkillCategory.Google, (int)GoogleSkills.CloudMemorystore);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudPubSub))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudPubSub), (int)SkillCategory.Google, (int)GoogleSkills.CloudPubSub);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudSpanner))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudSpanner), (int)SkillCategory.Google, (int)GoogleSkills.CloudSpanner);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudSpeech_to_TextAPI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudSpeech_to_TextAPI), (int)SkillCategory.Google, (int)GoogleSkills.CloudSpeechToTextAPI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudSQL))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudSQL), (int)SkillCategory.Google, (int)GoogleSkills.CloudSQL);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudStorage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudStorage), (int)SkillCategory.Google, (int)GoogleSkills.CloudStorage);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudText_to_SpeechAPI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudText_to_SpeechAPI), (int)SkillCategory.Google, (int)GoogleSkills.CloudTextToSpeechAPI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.CloudTPU))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.CloudTPU), (int)SkillCategory.Google, (int)GoogleSkills.CloudTPU);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.DataCatalog))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DataCatalog), (int)SkillCategory.Google, (int)GoogleSkills.DataCatalog);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.DialogflowEnterpriseEdition))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.DialogflowEnterpriseEdition), (int)SkillCategory.Google, (int)GoogleSkills.DialogflowEnterpriseEdition);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.FirebaseRealtimeDatabase))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.FirebaseRealtimeDatabase), (int)SkillCategory.Google, (int)GoogleSkills.FirebaseRealtimeDatabase);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleAppEngine))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleAppEngine), (int)SkillCategory.Google, (int)GoogleSkills.GoogleAppEngine);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleCloudContainerRegistry))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleCloudContainerRegistry), (int)SkillCategory.Google, (int)GoogleSkills.GoogleCloudContainerRegistry);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleComputeEngine))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleComputeEngine), (int)SkillCategory.Google, (int)GoogleSkills.GoogleComputeEngine);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleDataStudio))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleDataStudio), (int)SkillCategory.Google, (int)GoogleSkills.GoogleDataStudio);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleKubernetesEngine))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleKubernetesEngine), (int)SkillCategory.Google, (int)GoogleSkills.GoogleKubernetesEngine);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.GoogleSheets))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.GoogleSheets), (int)SkillCategory.Google, (int)GoogleSkills.GoogleSheets);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.Kubeflow))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.Kubeflow), (int)SkillCategory.Google, (int)GoogleSkills.Kubeflow);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.NaturalLanguage))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.NaturalLanguage), (int)SkillCategory.Google, (int)GoogleSkills.NaturalLanguage);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.PersistentDisk))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.PersistentDisk), (int)SkillCategory.Google, (int)GoogleSkills.PersistentDisk);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.RecommendationsAI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.RecommendationsAI), (int)SkillCategory.Google, (int)GoogleSkills.RecommendationsAI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.TensorFlow))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.TensorFlow), (int)SkillCategory.Google, (int)GoogleSkills.TensorFlow);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VideoAI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VideoAI), (int)SkillCategory.Google, (int)GoogleSkills.VideoAI);
            }
            if (!string.IsNullOrWhiteSpace(skillsetData.VisionAI))
            {
                await AddEmpSkill(empAssetDetail.EmployeeEntryID, empAssetDetail.EmployeeID, int.Parse(skillsetData.VisionAI), (int)SkillCategory.Google, (int)GoogleSkills.VisionAI);
            }
        }
    }



    public class SkillsetData
    {
        public string EmpID { get; set; }
        public string EmpName { get; set; }
        public int LocationID { get; set; }
        public string TotalExperience { get; set; }
        public string PrimarySkills { get; set; }
        public string SecordarySkills { get; set; }
        public string VisaStatus { get; set; }
        public string RLanguage { get; set; }
        public string SPSS { get; set; }
        public string SAS { get; set; }
        public string MachineLearning { get; set; }
        public string Kinesis { get; set; }
        public string Glue { get; set; }
        public string Athena { get; set; }
        public string BA { get; set; }
        public string JavaBigData { get; set; }
        public string DataPipeline { get; set; }
        public string Python { get; set; }
        public string Qubole { get; set; }
        public string CassandraBigData { get; set; }
        public string KafkaBigData { get; set; }
        public string HadoopBigData { get; set; }
        public string PigBigData { get; set; }
        public string CouchBase { get; set; }
        public string Neo4J { get; set; }
        public string MongoDB { get; set; }
        public string NoSQL { get; set; }
        public string GoogleBigQuery { get; set; }
        public string HBase { get; set; }
        public string Oozie { get; set; }
        public string Flume { get; set; }
        public string Hive { get; set; }
        public string Sqoop { get; set; }
        public string Scala { get; set; }
        public string MapReduce { get; set; }
        public string SparkDatabricks { get; set; }
        public string Storm { get; set; }
        public string AwsAdministration { get; set; }
        public string AWSEC2 { get; set; }
        public string AWSRedshift { get; set; }
        public string AWSElasticBeanStalk { get; set; }
        public string AWSLambda { get; set; }
        public string AWSVPC { get; set; }
        public string AWSIAM { get; set; }
        public string AWSRoute53 { get; set; }
        public string AWSSES { get; set; }
        public string Quicksight { get; set; }
        public string Elasticcache { get; set; }
        public string SQS { get; set; }
        public string Comprehend { get; set; }
        public string Aurora { get; set; }
        public string dynamoDB { get; set; }
        public string Stepfunctions { get; set; }
        public string DMS { get; set; }
        public string AWSSNS { get; set; }
        public string AWSCloudWatch { get; set; }
        public string AWSS3 { get; set; }
        public string AWSCloudFront { get; set; }
        public string AWSEFS { get; set; }
        public string AWSGlacier { get; set; }
        public string AWSRDS { get; set; }
        public string AWSCloudFormation { get; set; }
        public string AWSCloudTrail { get; set; }
        public string AWScloud { get; set; }
        public string AWSConfig { get; set; }
        public string AWSTrustedAdvisor { get; set; }
        public string AzureAD { get; set; }
        public string GoogleCloud { get; set; }
        public string Jenkins { get; set; }
        public string codepipeline { get; set; }
        public string codedeploy { get; set; }
        public string TeamCity { get; set; }
        public string Splunk { get; set; }
        public string RightScale { get; set; }
        public string Incapsula { get; set; }
        public string Chef { get; set; }
        public string Puppet { get; set; }
        public string BitBucket { get; set; }
        public string Ansible { get; set; }
        public string Terraform { get; set; }
        public string Joomla { get; set; }
        public string OpsWorks { get; set; }
        public string GitlabGithub { get; set; }
        public string SVN { get; set; }
        public string WordPressDrupalMagento { get; set; }
        public string TSQL { get; set; }
        public string PLSQL { get; set; }
        public string Informatica { get; set; }
        public string PerlScripting { get; set; }
        public string PentahoETL { get; set; }
        public string Snaplogic { get; set; }
        public string Mulesoft { get; set; }
        public string ODI { get; set; }
        public string TalendDI { get; set; }
        public string Attunuity { get; set; }
        public string DataStage { get; set; }
        public string SQLServer { get; set; }
        public string MySQL { get; set; }
        public string Postgres { get; set; }
        public string Redis { get; set; }
        public string Oracle { get; set; }
        public string Netezza { get; set; }
        public string Snowflake { get; set; }
        public string Teradata { get; set; }
        public string DB2 { get; set; }
        public string MainFrame { get; set; }
        public string SQLServerAdmin { get; set; }
        public string DBAdmin { get; set; }
        public string AdobeFlash { get; set; }
        public string AutoDesk3dsMax { get; set; }
        public string AutoDeskMaya { get; set; }
        public string AdobeAftersEffects { get; set; }
        public string Photoshop { get; set; }
        public string Magento { get; set; }
        public string WooCommerce { get; set; }
        public string CoreJava { get; set; }
        public string Json { get; set; }
        public string SpringFramework { get; set; }
        public string Hibernate { get; set; }
        public string JUnit { get; set; }
        public string Maven { get; set; }
        public string Ant { get; set; }
        public string Gradle { get; set; }
        public string Multithreading { get; set; }
        public string JMS { get; set; }
        public string MQ { get; set; }
        public string Webservices { get; set; }
        public string Intellijeclipse { get; set; }
        public string J2ee { get; set; }
        public string Ibatis { get; set; }
        public string EJB { get; set; }
        public string Struts { get; set; }
        public string JSP { get; set; }
        public string _Net { get; set; }
        public string BizTalk { get; set; }
        public string ASP_NET { get; set; }
        public string VB { get; set; }
        public string MSBICubes { get; set; }
        public string SharePoint { get; set; }
        public string SSAS { get; set; }
        public string SSRS { get; set; }
        public string SSIS { get; set; }
        public string VisualStudio { get; set; }
        public string CSharp { get; set; }
        public string PaymentGatewayIntegration { get; set; }
        public string SSOintegration { get; set; }
        public string Android { get; set; }
        public string CloudDevopsNOCServices { get; set; }
        public string ChefNOCServices { get; set; }
        public string LinuxNOCServices { get; set; }
        public string Astyanax { get; set; }
        public string Nagios { get; set; }
        public string Zabbix { get; set; }
        public string Haproxy { get; set; }
        public string Asterisk { get; set; }
        public string PRTG { get; set; }
        public string QA { get; set; }
        public string Jmeter { get; set; }
        public string Selenium { get; set; }
        public string UFT { get; set; }
        public string VisualStudioCodedUI { get; set; }
        public string VSTestManager { get; set; }
        public string QualityCenter { get; set; }
        public string MSDynamicCRMTesting { get; set; }
        public string SoapUI { get; set; }
        public string IBMRationalProducts { get; set; }
        public string SAPABAP { get; set; }
        public string SAPBW { get; set; }
        public string SAPBO { get; set; }
        public string SAPHana { get; set; }
        public string SAPBODS { get; set; }
        public string SAPLumira { get; set; }
        public string SAPDesignStudio { get; set; }
        public string SAPwebDynpro { get; set; }
        public string SAPLeonardoIOT { get; set; }
        public string ElasticsearchCloudsearchSolr { get; set; }
        public string Kibana { get; set; }
        public string Level1Support { get; set; }
        public string BashScripting { get; set; }
        public string Microstrategy { get; set; }
        public string Jasperreport { get; set; }
        public string Tableau { get; set; }
        public string IBMCognosTM1BI { get; set; }
        public string ExcelMacrosVBA { get; set; }
        public string ExcelDashboards { get; set; }
        public string Looker { get; set; }
        public string QlikviewQliksense { get; set; }
        public string PowerBI { get; set; }
        public string ApacheNginx { get; set; }
        public string TomcatServer { get; set; }
        public string JbossServer { get; set; }
        public string WeblogicServers { get; set; }
        public string HTML { get; set; }
        public string AngularJS { get; set; }
        public string Angular { get; set; }
        public string Javascript { get; set; }
        public string GWT { get; set; }
        public string nodejs { get; set; }
        public string ReactJS { get; set; }
        public string PHP { get; set; }
        public string XML { get; set; }
        public string AJAX { get; set; }
        public string CSS3 { get; set; }
        public string EJS { get; set; }
        public string RafaelJS { get; set; }
        public string CodeIgniter { get; set; }
        public string Rubyonrails { get; set; }
        public string Grunt { get; set; }
        public string AtmosphereFramework { get; set; }
        public string PlayFramework { get; set; }
        public string Jquery { get; set; }
        public string Bootstrap { get; set; }
        public string Postman { get; set; }
        public string Akka { get; set; }
        public string LoadRunner { get; set; }
        public string Naturallanguage { get; set; }
        public string Adabas { get; set; }
        public string JCL { get; set; }
        public string COBOL { get; set; }
        public string BigQuery { get; set; }
        public string CloudDataflow { get; set; }
        public string CloudPubSub { get; set; }
        public string CloudBigtable { get; set; }
        public string CloudStorage { get; set; }
        public string CloudDataproc { get; set; }
        public string CloudDataFusion { get; set; }
        public string CloudComposer { get; set; }
        public string GoogleDataStudio { get; set; }
        public string CloudDataTransfer { get; set; }
        public string CloudDatalab { get; set; }
        public string DataCatalog { get; set; }
        public string GoogleSheets { get; set; }
        public string CloudDataprep { get; set; }
        public string CloudSQL { get; set; }
        public string PersistentDisk { get; set; }
        public string CloudSpanner { get; set; }
        public string CloudMemorystore { get; set; }
        public string CloudFirestore { get; set; }
        public string FirebaseRealtimeDatabase { get; set; }
        public string GoogleComputeEngine { get; set; }
        public string GoogleAppEngine { get; set; }
        public string GoogleKubernetesEngine { get; set; }
        public string CloudFunctions { get; set; }
        public string GoogleCloudContainerRegistry { get; set; }
        public string AIHub { get; set; }
        public string NaturalLanguage { get; set; }
        public string AutoMLTranslation { get; set; }
        public string CloudSpeech_to_TextAPI { get; set; }
        public string CloudText_to_SpeechAPI { get; set; }
        public string DialogflowEnterpriseEdition { get; set; }
        public string CloudInferenceAPI { get; set; }
        public string RecommendationsAI { get; set; }
        public string BigQueryML { get; set; }
        public string TensorFlow { get; set; }
        public string VisionAI { get; set; }
        public string VideoAI { get; set; }
        public string AutoMLTables { get; set; }
        public string CloudAutoML { get; set; }
        public string AIPlatformNotebooks { get; set; }
        public string AIPlatformDeepLearningVMImage { get; set; }
        public string CloudTPU { get; set; }
        public string Kubeflow { get; set; }
    }

    #region Enumerations

    public enum SkillCategory
    {
        Analytics = 1,
        BA,
        BigData,
        CloudAWS,
        CloudAzure,
        CloudGCS,
        DevOps,
        CMS,
        ConfigManagement,
        ContentManagementSystem,
        DataIntegration,
        Database,
        DBA,
        Design,
        ECommerce,
        JavaStack,
        MicrosoftStack,
        Misc,
        Mobile,
        NOC,
        QA,
        SapBI,
        SearchAndAnalytics,
        Support,
        Unix,
        Visualization,
        WebServer,
        WebTech,
        Tools,
        Mainframe,
        Google
    }

    public enum AnalyticsSkills
    {
        RLanguage = 1,
        SPSS,
        SAS,
        MachineLearning,
        Kinesis,
        Glue,
        Athena,
    }

    public enum BaSkills
    {
        BA = 8,
    }

    public enum BigDataSkills
    {
        JavaBigData = 9,
        DataPipeline,
        Python,
        Qubole,
        CassandraBigData,
        KafkaBigData,
        HadoopBigData,
        PigBigData,
        CouchBase,
        Neo4J,
        MongoDB,
        NoSQL,
        GoogleBigQuery,
        Hbase,
        Oozie,
        Flume,
        Hive,
        SQoop,
        Scala,
        MapReduce,
        SparDatabricks,
        Storm,

    }

    public enum CloudAwsSkills
    {
        AWSAdministration = 31,
        AWSEC2,
        AWSRedshift,
        AWSElasticBeanStalk,
        AWSLambda,
        AWSVPC,
        AWSIAM,
        AWSRoute53,
        AWSES,
        QuickSight,
        ElasticCache,
        SQS,
        Comprehend,
        Aurora,
        DynamoDB,
        StepFunctions,
        DMS,
        AWSSNS,
        AWSCloudWatch,
        AWSS3,
        AWSCloudFront,
        AWSEFS,
        AWSGlacier,
        AWSRDS,
        AWSCloudFormation,
        AWSCloudTrail,
        AWSCloud,
        AWSConfig,
        AWSTrustedAdvisor,
    }

    public enum CloudAzureSkills
    {
        AzureAD = 60,
    }

    public enum CloudGcsSkills
    {
        GoogleCloud = 61,
    }

    public enum DevOpsSkills
    {
        Jenkins = 62,
        CodePipeline,
        CodeDeploy,
        TeamCity,
        Splunk,
        RightScale,
        Incapsula,
        Chef,
        Puppet,
        BitBucket,
        Ansible,
        Terraform,
    }

    public enum CmsSkills
    {
        Joomla = 74,
    }

    public enum ConfigManagementSkills
    {
        OpsWorks = 75,
        GitlabGithub,
        SVN,
    }

    public enum ContentManagementSysSkills
    {
        WordPressDrupalMagento = 78,
    }

    public enum DataIntegrationSkills
    {
        TSQL = 79,
        PLSQL,
        Informatica,
        PerlScripting,
        PentahoETL,
        Snaplogic,
        Mulesoft,
        ODI,
        TalendDI,
        Attunuity,
        DataStage,
    }

    public enum DatabaseSkills
    {
        SQLServer = 90,
        MySQL,
        Postgres,
        Redis,
        Oracle,
        Netezza,
        Snowflake,
        Teradata,
        DB2,
        MainFrame,
    }

    public enum DbaSkills
    {
        SqlServerAdmin = 100,
        DbAdmin,
    }

    public enum DesignSkills
    {
        AdobeFlash = 102,
        AutoDesk3dsMax,
        AutoDeskMaya,
        AdobeAftersEffects,
        Photoshop,
    }

    public enum ECommerceSkills
    {
        Magento = 107,
        WooCommerce,
    }

    public enum JavaSkills
    {
        CoreJava = 109,
        Json,
        SpringFramework,
        Hibernate,
        JUnit,
        Maven,
        Ant,
        Gradle,
        MultiThreading,
        JMS,
        MQ,
        WebServices,
        IntellijEclipse,
        J2ee,
        Ibatis,
        EJB,
        Struts,
        JSP,
    }

    public enum MicrosoftSkills
    {
        DotNet = 127,
        BizTalk,
        ASP_NET,
        VB,
        MSBICubes,
        SharePoint,
        SSAS,
        SSRS,
        SSIS,
        VisualStudio,
        CSharp,
    }

    public enum MiscSkills
    {
        PaymentGatewayIntegration = 144,
        SSOIntegration,
    }

    public enum MobileSkills
    {
        Android = 146,
    }

    public enum NocSkills
    {
        CloudDevopsNOCServices = 147,
        ChefNOCServices,
        LinuxNOCServices,
        Astyanax,
        Nagios,
        Zabbix,
        Haproxy,
        Asterisk,
        PRTG,
    }

    public enum QaSkills
    {
        QA = 156,
        Jmeter,
        Selenium,
        UFT,
        VisualStudioCodedUI,
        VSTestManager,
        QualityCenter,
        MSDynamicCRMTesting,
        SoapUI,
        IBMRationalProducts,
    }

    public enum SapBiSkills
    {
        SAPBW = 166,
        SAPBO,
        SAPHana,
        SAPBODS,
        SAPLumira,
        SAPDesignStudio,
        SAPABAP,
        SAPWebDynpro,
        SAPLeonardoIOT,
    }

    public enum SearchAndAnalyticSkills
    {
        ElasticsearchCloudsearchSolr = 175,
        Kibana,
    }

    public enum SupportSkills
    {
        LevelOneSupport = 177,
    }

    public enum UnixSkills
    {
        BashScripting = 178,
    }

    public enum VisualizationSkills
    {
        Microstrategy = 179,
        JasperReport,
        Tableau,
        IBMCognosTM1BI,
        ExcelMacrosVBA,
        ExcelDashboards,
        Looker,
        QlikviewQliksense,
        PowerBI,
    }

    public enum WebServerSkills
    {
        ApacheNginx = 188,
        TomcatServer,
        JbossServer,
        WeblogicServers,
        IIS,
    }

    public enum WebTechnologySkills
    {
        HTML = 193,
        AngularJS,
        Angular,
        Javascript,
        GWT,
        NodeJS,
        ReactJS,
        PHP,
        XML,
        AJAX,
        CSS3,
        EJS,
        RafaelJS,
        CodeIgniter,
        RubyOnRails,
        Grunt,
        AtmosphereFramework,
        PlayFramework,
        Jquery,
        Bootstrap,
        Postman,
        Akka,
    }

    public enum ToolsSkills
    {
        LoadRunner = 215,
    }

    public enum MainframeSkills
    {
        NaturalLanguage = 216,
        Adabas,
        JCL,
        COBOL,
    }

    public enum GoogleSkills
    {
        BigQuery = 220,
        CloudDataflow,
        CloudPubSub,
        CloudBigtable,
        CloudStorage,
        CloudDataproc,
        CloudDataFusion,
        CloudComposer,
        GoogleDataStudio,
        CloudDataTransfer,
        CloudDatalab,
        DataCatalog,
        GoogleSheets,
        CloudDataprep,
        CloudSQL,
        PersistentDisk,
        CloudSpanner,
        CloudMemorystore,
        CloudFirestore,
        FirebaseRealtimeDatabase,
        GoogleComputeEngine,
        GoogleAppEngine,
        GoogleKubernetesEngine,
        CloudFunctions,
        GoogleCloudContainerRegistry,
        AIHub,
        NaturalLanguage,
        AutoMLTranslation,
        CloudSpeechToTextAPI,
        CloudTextToSpeechAPI,
        DialogflowEnterpriseEdition,
        CloudInferenceAPI,
        RecommendationsAI,
        BigQueryML,
        TensorFlow,
        VisionAI,
        VideoAI,
        AutoMLTables,
        CloudAutoML,
        AIPlatformNotebooks,
        AIPlatformDeepLearningVMImage,
        CloudTPU,
        Kubeflow,
    }

    #endregion
}



