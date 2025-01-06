namespace AdventOfCode.Puzzles.Day9;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;

[MemoryDiagnoser(false)]
[SimpleJob(RuntimeMoniker.Net90, baseline: true)]
public class Puzzle2
{
    [Test]
    [Benchmark]
    public void Solve()
    {
        var files = new List<(char Id, int Size, int FreeSpace)>();
        for (int fileId = 0, i = 0; i < PuzzleInput.Length; i += 2)
        {
            var size = PuzzleInput[i] - '0';
            var freeSpace = i + 1 < PuzzleInput.Length ? PuzzleInput[i + 1] - '0' : 0;
            files.Add(((char)(fileId++ + '0'), size, freeSpace));
        }

        var diskSize = files.Sum(x => x.Size + x.FreeSpace);
        Span<char> disk = stackalloc char[diskSize];
        disk.Fill('.');

        for (int idx = 0, i = 0; i < files.Count; i++)
        {
            var (id, size, freeSpace) = files[i];
            var slice = disk.Slice(idx, size);

            slice.Fill(id);

            idx += size + freeSpace;
        }

        for (var i = files.Count - 1; i >= 0; i--)
        {
            var (file, size, _) = files[i];
            var idx = disk.IndexOf(file);

            for (var j = 0; j <= idx - size; j++)
            {
                var slice = disk.Slice(j, size);
                var empty = true;

                foreach (var c in slice)
                {
                    if (c is not '.')
                    {
                        empty = false;
                        break;
                    }
                }

                if (empty)
                {
                    for (var k = 0; k < size; k++)
                    {
                        disk[j + k] = file;
                        disk[idx + k] = '.';
                    }
                    break;
                }
            }
        }

        var checksum = 0d;

        for (var i = 0; i < disk.Length; i++)
        {
            if (disk[i] is not '.')
            {
                checksum += i * (disk[i] - '0');
            }
        }

        Console.WriteLine($"The checksum is {checksum}");
    }

    private const string PuzzleInputTest = "2333133121414131402";

    private const string PuzzleInput = """
        2841627148755595347924125852582654605174565397553360307432589096303483571124734584586210354123601330562823412141224024641070135033171488643018485383593864593613944290247592914710571182835389418251596615864128489342335128113774221660213974216969398155455112866521431943259293819242686263845488731685111619229729517931885686874442886455721061212991835568306211134681185190383257271117107918166185933079346964864898663615467531495079149063124634998634345647489411407328598862985320436896267494488838418070513724661271424172204156792316894989168380835935996262545396512884409535431824504121143434862992281365504281523615342988701914411617238513395510289969759770732710332816893238125637802991404645271621563473138686868240673337448678613876266290319988464080276680328055548344824897917086424986689459238298618096717045554218298599258743701765545313786129578155627671869329264847775649541323598127741575384571823336938166589296593337501444601240743664496081536374414879571291211029838158182677236755899041153683222744799487466713592266635314206553729738316947403691798023415427348431415891332538143325801272494165537296385712205071262681693065713143205766925596767581152564462226489098437910234474628688262731866667154496638830669512853216657353589436599236948153293811867272412859637679991222744248927445785926869449635294609564386520313682568664833625301394346126526787469656496542421146604293985245113055829255189140658323144422274914247057761846768682821159841072295813284887935817441382359419697418924323319947726113367334259080955524908515875157906482457061644150235557844747956876886528449273965533505312332657582727521069887294372730299273637444927872395738161298442792866920675616457378445643235530516638109811993852864376259674535623929518715710159084125350161272993436885091653344604987727538496120296641493357547434679215475535774479985238172557131999922511565427915028453179298233814079123236364768382746786577267088513562703710936681959197955579234026964170387282257035588615902241511456661271138340365830323712277466195454262556914572215772811956652936557129131772401257857816629745537472983791247250351983393063964810294313424755698873768932503488304468184121487239841340391973348037982328486236356518468268157489934654653290751517901461402937873820144221346274468352588142494356454039553683828539838428623163779216952744311240132448274973641888161774838093207791877384119528236041972395771611278375353482384565744018126983255585528618126329645861198054461572226893211341919528163716318036201329469092617282267329952218239967126876868977542056617689488231175267465616974893674248397248518882576261406095167467296620813424835754886012596243875385618946211925505168531577775164602365806271456025526130687934346928653663994210172017495881969388163144159297219747954667856013616158498399635596647827613683662429173353208071627949935839936219239616192372261388225860536329429180567566603537527461613029116964232947291013678078465377599132246022578464408064174220774980387450825552306481785178655149662972371744252262318871392876949186412862716919399644731479784016845247579825605122999187173527935729497060121586904568274616529325926851731672195599827092701353535433327057745889558539769457227459236473538962909289635027136923415167425318195920679445969043901662431673204552632784139694333149982385319280302451203491909239313110748148189044395992655346683265218614838345245978853750985037878030335362103294806348564647296113335233324045899692346940916429391957546869823113264943391812248419771771766285809778182390559536561377509136991588727530546578966964744245733464151851582177148882157221708827743774864264252768649752141718452092795213791767394750873813583210571340804481872345233151215885876831798491775536722218642724201330276983536146216747241196928738332010909481896330467848444192592663498769658229504528451750938814281271136055948684353270458690378619505669382983206275869951621590506167869512563274489319637663389253159420937149524097997767729242814542687797478885315413531566423364996520185193728846862050712167319028184275585480731025928548946323788195827783855920576368912839479059838439232067426190998778604734438774861898712214152035288338284447291444112894123392214316634179638130657091611226941322259543261331333986471759688175269813717719285638756039507852759611977312375047181890751172759363338817974923647665568458577676472686906550867611724746919060882515795377732536429996194460738484404869224275323563398065886451574725569875743923282771745782674193948972492793447529316087221128693036158851625773637511913511566450748068803563269069581926642526998737921630893010581032902795211042712850909899524893383220633267342039397897124921944944954146216144678755118276261189753173542263782142806076244164122581534573549739253811369226834428875148879538507195208191357829182925881516652863229570434971798689595960289092588792983331684480347135848793683676842065318032144457662077483974516741621442252568554236844924651718142844148681132499732941677828325389787911502855785746173559996115692461328238224531531988774958145132647140332830513823593444259930473247554685622266545966453073971786129415856867647835486064423198428163303333994619595294748620822177652863542895958014516348913671636922886824112928363254852634865186868970857519169339868873964080166517578557822397142716614254667359109727503914785637494328573750724141438091918897628651837394932246248035702448682047505531165293977443451929375039383595906641805872869114346175511397129828464998855913902370686495363548888260566091211717521029641258548416668890497869855327665010653310341733944357528490539027979845256092575731736131103174722262529276127616108021833216616855654442911650519112448095937933653212976194345928579692689165474450875557574996675081763173477170355850663750524864363183883651768435382945866664755342669213979421969020978667787428392055757187126476188239683665535020491576746356481568558273529452359470219459229686256221417361892589405129503519674260359694453260452946387977844844548618677218758288668577348599942186276495486336439794206575232256966722557945107156116348705316294916244971598468163173348673552483298882604489105371996155808925602423577223114824781080967178782251707614264894244650962512784782584334404513623129359248327357628437742622536542644833763178656190834833401137736225651499745090908439833699175641776554876917845747323857485889261248596883484447185873393190704997792925177078166768888525311893219725159522162913734853698178374592723460197577239622866955261622258763796879983264319769348361801182534474708244198166971769462524614925809518373464417677944292544490487573557260216214181612213092592958391246535481341817508112462072122769652585301954542691676776259267514838274086909390625081524843367470974299676721637413646342227637941621414630143568817815438644328938316866549481239213779491159320549421122275117890513343961043971497791855138017154946636911478872978716956066191622995634124374121188836548227386905610875476687658453485233628501552957040925823878335164190944251331736685865911079777090657554572089223042107952628177822127564069694793292288118338446358781037994361932774986554231095147069577685644049831887922575211282582884408336753692643920407718885683975567368843812380798984458437643657646566419188817526846492125995715475122518587079709815971737408452325832788768464014684164126152106097865793745018307218728933543973901974342092361896153152769477491225248053608991661040132146228521792898172361902412537914245597537948984582528717426288466111259918707249933576107812507916136271257277999346984916788227711092192045478227465025983398748316841015914634422397814693352638678881613959607683379389814882775338196397826677121189749798419697378527603535589569253855238263862217108161375232561219921543706126774948592937349316208769644854214912782195344484381781643594251626669372189851174182163186589971972662827426792345316113396739986490663744738226519332738070538099508130364333952493742891628561497756515758286730341732729156821447135331642557114197856337189046602525843817619916473369473448632664938314415160945790176743191418681634687119874989753259521661944843629988175488633739407179891154757170285128836491237159892721718328552817371852156427822015248484681080725256753619574488392598997963737265312639235878943376525930237567615050707257849420356948532782267089368267513651133191929413532590778419764561218111399041488565565128327315223762334947416362665448756076327377273995197999467197972456885327743259291277215861773662388822993476609957416616564569301715843015442032945089899443785088105677481216455288732414838787236195896935221698813486967893869282927396692653113464997152772440725493783625622089675645911939567179467929791057145112754632937768949759262776888087954566202433608956991721279753349266803288501145867940445690891986715871299239455380899516591727533641225447222985981457896131273067173674994031487911722192996770206178528551412237274249273034973340334574926378871970515847868775661260876092822243988714665950975282401912113694577637251019769444665112818892549975887853377470509444334591765157171049745786572216111716107248699452849785662992371932688256566064196884246063747529971726255729596492594486824551839167568817936878623449456025977465919221781710935695859932854223471864174236891047429432254792889312895811209184489684419837141855222424322338642567382890927459135041444511719727892738256767426298592261402988624238253280243615115664121158364898976767833492735129358361298110596697494225398351969478608584313946756957965379686565495952682540854977115263157019742696888880729040487342429945373982444370886116266378583640704079765767646642373830561010788244192523254435251133748122825714967473393189595831583249618798501118826127442517801611495982624410929323608718309335276476118111325115853282724871161357232545474166113648986081229677104563717649174631747036441646137912616992199468433151174735824089796394958476781431857661817317569978736511145232938397181072181192205156527427597031313345648961246336815637763528894921281388553043827086877256143416329397399879478466111270355321409647919684309630997011642745502018953417476067195536387162442569132987785870961544943047321969625612403937871586103163505875575387209569715228703419508577489259815576855534636020703090125945174713624685349033569776553430342770926662547280565517737144413545328046926152102375781520664817679264276528942714864637185944923123205961226835213685138932142350245737333480834344148626289782289283554837299842561713745576349665122061211589405267245189475767129436988267352617502283172884185433839791533738536174383931808490834357111542196482102575254111894021979857604454854339125434285746787751825770918476252290325354665482581714875087408821168864173066878315994294346948995641856011846422695684649073288252881635934184673667578897559646926336257335993727113434573347937856713338494680318575376569149236445256363293408356872839649153257551711064897030446811951639864565448765494991167041478577342179284038512538523631783944823792113623494285471352385170866079404958232278458497659946897541385075923170929958488124491112397052152486789612507211203114679159198289432636272783947075365663899539947947431890287360204949364771885582953727636440916747682723785734767642824330619658314928675470869666889177619796816780618655405519424353516093376389129427589590877729768794469093168487532393279116332964376553545387704639625256171171604654253089707722588852289371135659609886906398387010119763343373283261842248117966466222942193355492381829843146351589112521193279406716894790107570865786572913919671823126153247693790255788635020437929176474932328539168844160639783218313171545231832674229553534565721136933662431741453947885689590606846695397853154348765119163904165982721343410525665311753669319633477953766207267108842512937337387175367122428605647447670487779146728518068334276978296944636809749721265421342634420726647658110244832195633552683296526415429323455795799904327284021647950616560836992868071114754963070929535691448231438424391651619708598985369612043211421965020836465377196962241893522718035306517793391556172783993746161464450137868608045226449543613898334334525323717907158229337502031156932394251707228649562391540615786969999445964108912663728375557345724142363285944525927317431578428618788613588724880745580256766106939712572833354878320447568298373267087457943672816839312318292941556465616493851473358662751637234772368761858877577574151206171715651203734465950855754148353704163874627305321879276579929263994359080486540568131346374995257331357734313389778549234827332623765466171785375131059224446281658931042985282872966487053693597494088387977275554249584181043389513663694897448939699846490327195378374341769963392372569144422901878985243971149134696165025444542723950826227681273314012591966641366997246748240365824336140399448354540922294468793716310233133102289987376411128885343222325628599212889631830776665888128528460317355688560157060756777843687738282132945902970821076948269738262223642708493319221619766257741579852723714557851786012451722303394422549256778633786347124427137425397195578579183891411195863611917281968297998209159706797712991258094352049968186208329358366579660513136521472851962622321486795872181443375128340629731432197365128347822762913961890304381993860226031848151617749143675989684577222808840973232722341218020947769136789147096817240598322451811964219134296734182242357521797193192688270834051761655532768692832537995267375919076973451489431871085348550587181707260116426162296879482244920824358593039372744413860653773665779582028165164705464689623642380644712364311297850194838447830156097869513646295849826299154225189641864606920999533641978711744533993562257533262987550305378807920987777909391907497725769582491137780335097991615189733375177897386547683825635186497131414528770289418888032356683116726404420895772739624802499238372925662797876289258526962833135598262147626864523166938275868678838794189499426498649266515173777932550843924968027329957843851788312429832202519682979364658585016728176996980776851868559781888621926499485428848879727931091626333322184791093903380891557463738911960821793976195622097343568786142845919963912365111207442527177602420731847615997689624101263864054181765277875592792991534165885494152673338561288513888114559195476462463669163351154227494955120116864916755406345584727616542716456388149282767697551622993598530216135338871547597897348857725109136302486443954797656788973141386143090381057933541439067499387543511112119782553294550522717733070793889244813318093993342303993286321565487638634923069385846485231691820252190954247158271314719388951876473898829884920918662299070471592162915769280843268186061617666414667724020257480869385225797551928996226951428343366943744993069745210839561493218451796931739754160504114465238723986711329331274683717383596283243772693976217429346603130224699279138696930652493534974992095881112895955837233604030847991259599348871566156106752151543699469657981655139601310448468541515502713372093995032956597578489509254251948357667948037326474465150631565144244691378343465773356911141448494142040735567131880803413507958756468475686519613391616522937634539518291582610927923408288375223547122341645646940987225773579792077115360685253144997265493192250935482549452211247429651362246949076148194114581801755358276669868271214612127882728449149409081895711132245576677539595932026306085209847478639869376905876919762827239122118322085757426795434641531535216505423978076468119246788377137106023447252377157217351679036139327191380478681984086778548354428225036155530145978168575614946743976847863716171469954386295456057705314266292381059434820612122216116566337483672441457523186933544341548812332717680454749748280902723763488788880359871795758628465173915733994545751877349992426865956514272369617239416794559235547661538522846263554141983217332254585392684432137692271123641239955529860368385807856751946718393636330444840375464701825181615302384484410351254988226504622737272894614138098302146736028272275356061976742679618666929902740872729524129611243886218421461333118696786305486449578232638141374932390649537993083799966878955503071294384457160575171698874646688955893421644926890777998897764398930629041737923566983817736258236785963641369407418362626229051265458252728505850776861146712394649509168132695936916964123704697113989869961979531728572194230226416993654249567769156167640269224175490129666877753588816706768974288352136559132301768657770873199841786605314415878285736576154672365672511535184466914467056207768896725412837892812569895699442815056702943408631157812216125794035355374304861359913135440845527258688381158447880131649823726885046691574373885819084595439921333971375507671951950801910435158302053201958677990222554392799912985883296705528821795807514664319341459789142129581683355558798588964255671661448371532626244939358466315117164978556405945812065112960205845712316849025936351135552616875214664801612217775987750434224657516137490788719908258458077416039461280579064312033692598101391392294133281971424528179642546116675822594655773663558648499316092948949388091579250318517829287602147252670897388991218697921458620571313912086641564397294293154145899121636251034865591454614597813849893648493692758754911747730194474668044538773827789166991196961105718516714961341173019162391664338263515141553548579295765961975457215383986276490215760815835301440685962277327876231676442159310414647862135774846215077264327475844568664973498354045218327995096619728618789929616488228741112506366579084889117531360591371753458191349703836408466311352639397912946114249704276246224533450234560488739812781714882287287899820597214473780502094795091585588535391923417492142948284911083698898397366758020887028829813235667407881752774139565591142797236505272891754374735735888208036916487301483468126957739385428966688844564282989252174941693209675114273377997174854542775768448362453872475415353694236927490237686148785865479867795615833345192573164207988966587938681309120254029644189397217603862245557335828906668387263356017845747819216292141189821477181165612421047404631571173669825967891953915108275714182405622919918922614709246742618587624199837761171641516873193331155243466123484159145785213775232323195444237421333926983963854962039207465185746841663399475275312861445438361322585728789523036118061967861739754709199908072552098938411135927302592299470632348641642317990252494486041265439972068621815155014909818734749711244633397552855981968661867963170577396224881458818485535459087805627756994612145841038944839758823648678718354192745828142278852541947142455323126698520403074746360847849342193403248719492503116479760973497739581591198853897356773266726797825991016339616946448679844954841986932535862582110706922599686191316481633172719152874411112861925582439944037902182404493723984916420164975793712744963472532462443419038652863677639287198308363916135357022359972568481563626529382392180548238724829236624287888414222852885645951422914271685357157216989458160478324383845389595203953644845361983552484908670268066831047412757715965505527381191181610807168886969827815786242409983902925775653759376649499444187846767678182249288544066436088236978233676127935552540937256711559511450341773424415636820953278788957208434416254758768233669537216835938591771127027124233832176153469547814456463169115336339691199956533875793895260502573354271138859379526568996546136702394118644638262586962797765312745306065384165803625774763366143666419393720611737353824162939282411686066788936547286889953328513988832761124292890136323324193138173494977953834559987189918646125731065431119242990489964383999528063603152671776661980592098243646797681377261112238359425912573693764137268408542361466135311634770653578458189932112156613903517254672796628367917554840729843278029667852498188567351973982872647928676143544472246477610211546965024237611418262743722713958436799675881196553524248602891753534773781807041416063792746857645875791516344909663814160401864522813722662129376305
        """;
}
