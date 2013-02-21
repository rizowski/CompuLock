class CreateHours < ActiveRecord::Migration
  def change
    create_table :hours do |t|
    	t.references :day
    	t.integer :start_time
    	t.integer :end_time
      	t.timestamps
    end
  end
end
