using ROLAP.Configuration.Loaders.Base;
using ROLAP.Configuration.Models.Models;
using ROLAP.Processor;
using ROLAP.Processor.Interfaces;

IProcessor processor = new Processor(new CubeConfigurationStore(new CubeConfigurationLoader()));
processor.ProcessQuery("");