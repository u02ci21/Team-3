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

    water(tools) {
        if (tools !== 'watering can')
            return 'You need a watering can to water the plants.';

        if (this.condition !== 'planted')
            return 'You can only water after planting seeds.';

        this.condition = 'growing';
        setTimeout(grow, 10000); // wait 10 seconds for the plant to grow
        return 'Plants are growing. Please wait.';
    }

    grow() {
        if (this.condition !== 'growing')
            return;

        this.condition = 'grown';
        //maybe call a function that allows the image to change on phaser? 
        return 'Plants have grown! You can now harvest them.';
    }
    



}

