class CreateHours < ActiveRecord::Migration
  def change
    create_table :hours do |t|
    	t.references :day
    	t.integer :start
    	t.integer :end
      	t.timestamps
    end
  end
end
