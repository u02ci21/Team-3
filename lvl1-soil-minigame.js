//import player inventory file 

class Soil {
    constructor(condition, timer) {
        this.condition = 'dirty'; // conditions - dirty, cleaned, planted, grown
        this.timer = null; // timer to track growth
    }

    clean(tools) { 
        if (tools !== 'shovel')
            return 'You need a shovel to clean the soil.'; 

        if (this.condition !== 'dirty')
            return 'Soil is already cleaned or planted.';

        this.condition = 'cleaned';
        return 'Soil has been cleaned. You can now plant the seeds.';
    }

    plant(seeds) {
        if (tools !== 'seeds')
            return 'You need seeds to plant in the soil.';

        if (this.condition !== 'cleaned')
            return 'Soil must be cleaned before planting seeds.';

        this.condition = 'planted';
        return 'Seeds have been planted. Wait for them to grow.';
    }


}

